using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDieState : ZombieState
{
    float timeToDestroy = 5;
    float counter = 0;
    public ZombieDieState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {
    }

    public override void Awake()
    {
        base.Awake();
        _zombie.animator.SetTrigger("Die");
    }

    public override void Execute()
    {
        base.Execute();
        counter += Time.deltaTime;
        if (counter >= timeToDestroy)
        {
            GameObject.Destroy(_zombie.gameObject);
            counter = 0;
        }
    }
}