using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieIdleState : ZombieState
{
    float idleDuration = 5;
    float currentIdleDuration = 0;
    public ZombieIdleState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {
    }

    public override void Awake()
    {
        base.Awake();
        idleDuration = Random.Range(_zombie.minIdleTime, _zombie.maxIdleTime);
        currentIdleDuration = 0;
        _zombie.animator.SetFloat("Speed", 0);
    }

    public override void Execute()
    {
        base.Execute();
        currentIdleDuration += Time.deltaTime;

        if (currentIdleDuration < idleDuration)
        {
            _sm.SetState<ZombiePatrolState>();
        }
    }
}