using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRangeAttackState : ZombieState
{
    public ZombieRangeAttackState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {
    }

    public override void Awake()
    {
        base.Awake();
        _zombie.animator.SetTrigger("RangeAttack");
    }

    public override void Execute()
    {
        base.Execute();
        _zombie.transform.LookAt(_zombie.player.transform);
    }
}