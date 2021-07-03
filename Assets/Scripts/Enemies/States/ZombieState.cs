using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZombieState : State
{
    protected ZombieIA _zombie;
    public ZombieState(StateMachine sm, ZombieIA zombie) : base(sm)
    {
        _zombie = zombie;
    }
}
