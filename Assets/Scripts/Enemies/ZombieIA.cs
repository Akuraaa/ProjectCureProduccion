using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIA : MonoBehaviour
{
    [Header("Zombie Stats")]
    public int health;
    public int damage;
    public float speed;
    public float minIdleTime = 3;
    public float maxIdleTime = 5;
    public float fireRate = 1.5f;
    public float viewAngle = 45;
    public float rangeDistanceAttack;
    public float meleeDistanceAttack;
    public float radius;
    public float invulnerabilityTime = .5f;
    public float timeToDie;
    public float turnSpeed = .01f;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Quaternion rotGoal;

    [Header("Zombie Components")]
    public Transform axeSpawn;
    public GameObject axePrefab;
    public LayerMask _lm;
    public LayerMask groundMask;
    public FPSController player;
    public Animator animator;
    public SphereCollider visionRange;
    public StateMachine sm;
    public EnemyDecisionTree enemyTree;
    public AudioSource audioSource;

    [Header("Zombie Waypoints")]
    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypointTarget = 0;

    [Header("Zombie booleans")]
    public bool isRange;
    public bool playerInRange = false;
    public bool playerInMelee = false;
    public bool playerInSight = false;
    public bool view;
    public bool _isDead = false;
    private bool _isReceivingDamage = false;

    [Header("Zombie Audioclips")]
    public AudioClip meleeAttack;
    public AudioClip rangeAttack;
    public AudioClip receiveDamage;

    private void Awake()
    {
        sm = new StateMachine();
        sm.AddState(new ZombiePatrolState(sm, this));
        sm.AddState(new ZombieIdleState(sm, this));
        sm.AddState(new ZombieSeekState(sm, this));
        sm.AddState(new ZombieMeleeAttackState(sm, this));
        sm.AddState(new ZombieRangeAttackState(sm, this));
        sm.AddState(new ZombieDieState(sm, this));
        animator = GetComponent<Animator>();
        player = FindObjectOfType<FPSController>();
        enemyTree = new EnemyDecisionTree(this);
        enemyTree.SetNodes();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        enemyTree._init.Execute();
        radius = visionRange.radius;
    }

    private void Update()
    {
        sm.Update();
        if (view)
        {
            if (visionRange.radius < radius * 2)
            {
                visionRange.radius *= 2;
                viewAngle *= 1.5f;
            }
        }
        else
        {
            if (visionRange.radius == radius * 2)
            {
                visionRange.radius /= 2;
                viewAngle /= 1.5f;
            }
        }


    }

    public void TakeDamage(int amount)
    {
        if (health > 0)
        {
            health -= amount;
            audioSource.PlayOneShot(receiveDamage);
            speed = 0;
            animator.SetTrigger("ZombieHit");
            enemyTree._init.Execute();
        }
        if (health <= 0)
        {
            ActionDie();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<FPSController>())
        {
            playerInRange = true;
            player = other.GetComponent<FPSController>();
            LineOfSight();
            enemyTree._init.Execute();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FPSController>())
        {
            playerInRange = false;
            playerInSight = false;
            enemyTree._init.Execute();
        }
    }

    bool LineOfSight()
    {
        Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle)
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, dirToPlayer, out hit, visionRange.radius, _lm))
            {
                playerInSight = hit.transform.gameObject.layer == 8;
            }
            else
            {
                view = false;
                return false;
            }

        }
        return playerInSight;
    }

    public void ActionSeek()
    {
        sm.SetState<ZombieSeekState>();
    }

    public void ActionPatrol()
    {
        sm.SetState<ZombiePatrolState>();
    }

    public void ActionDie()
    {
        sm.SetState<ZombieDieState>();
    }

    public void ActionIdle()
    {
        sm.SetState<ZombieIdleState>();
    }

    public void ActionMeleeAttack()
    {
        sm.SetState<ZombieMeleeAttackState>();
    }

    public void ActionRangeAttack()
    {
        sm.SetState<ZombieRangeAttackState>();
    }

    public bool QuestionMeleeAttack()
    {
        //view = true;
        return Vector3.Distance(transform.position, player.transform.position) < meleeDistanceAttack;
    }

    public bool QuestionRangeAttack()
    {
        //view = true;
        return Vector3.Distance(transform.position, player.transform.position) < rangeDistanceAttack;
    }

    public bool QuestionIsPlayerOnSight()
    {
        return LineOfSight();
    }

    public bool HaveLifeQuestion()
    {
        return health <= 0;
    }
}
