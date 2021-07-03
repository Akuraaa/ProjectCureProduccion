using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIA : Target
{
    public bool isRange;
    public float minIdleTime = 3;
    public float maxIdleTime = 5;
    public float fireRate = 1.5f;
    public float viewAngle = 45;
    public float rangeDistanceAttack;
    public float meleeDistanceAttack;
    public float radius;
    public float spped;

    public Transform axeSpawn;
    public GameObject axePrefab;
    public LayerMask _lm;
    public FPSController player;

    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypointTarget = 0;

    public bool playerInRange = false;
    public bool playerInMelee = false;
    public bool playerInSight = false;
    public bool view;

    public Animator animator;
    public SphereCollider visionRange;

    public StateMachine sm;
    public EnemyDecisionTree enemyTree;
    public AudioSource audioSource;

    public AudioClip meleeAttack, rangeAttack;

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
        view = true;
        return Vector3.Distance(transform.position, player.transform.position) < meleeDistanceAttack;
    }

    public bool QuestionRangeAttack()
    {
        view = true;
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
