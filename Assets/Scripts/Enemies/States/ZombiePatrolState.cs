using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePatrolState : ZombieState
{
    public ZombiePatrolState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {
    }

    public override void Awake()
    {
        base.Awake();
        _zombie.animator.SetFloat("Speed", 0.5f);
    }

    public override void Execute()
    {
        base.Execute();
        Transform target = _zombie.waypoints[_zombie.currentWaypointTarget];
        _zombie.transform.LookAt(target);

        if (Vector3.Distance(_zombie.transform.position, target.transform.position) < .5f)
        {
            if (_zombie.currentWaypointTarget < _zombie.waypoints.Count - 1)
            {
                _zombie.currentWaypointTarget++;
            }
            else
            {
                _zombie.waypoints.Reverse();
                _zombie.currentWaypointTarget = 0;
            }
        }
    }
}