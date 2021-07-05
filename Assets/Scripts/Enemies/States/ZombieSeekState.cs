using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSeekState : ZombieState
{
    public ZombieSeekState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {
    }
    public override void Awake()
    {
        base.Awake();
        _zombie.animator.SetFloat("Speed", 1);
    }
    public override void Execute()
    {
        base.Execute();
        _zombie.direction = new Vector3(_zombie.player.transform.position.x, _zombie.transform.position.y, _zombie.player.transform.position.z);
        _zombie.transform.rotation = Quaternion.LookRotation(_zombie.direction - _zombie.transform.position);
        _zombie.transform.position += _zombie.transform.forward * _zombie.speed * Time.deltaTime;

        if (_zombie.isRange)
        {
            if (_zombie.QuestionRangeAttack())
            {
                _sm.SetState<ZombieRangeAttackState>();
            }
        }
        else
        {
            if (_zombie.QuestionRangeAttack())
            {
                _sm.SetState<ZombieMeleeAttackState>();
            }
        }
    }
}