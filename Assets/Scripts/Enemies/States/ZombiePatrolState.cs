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

        _zombie.direction = new Vector3(_zombie.waypoints[_zombie.currentWaypointTarget].position.x, _zombie.transform.position.y, _zombie.waypoints[_zombie.currentWaypointTarget].position.z);
        _zombie.transform.rotation = Quaternion.LookRotation(_zombie.direction - _zombie.transform.position);
        _zombie.transform.position += _zombie.transform.forward * _zombie.speed * Time.deltaTime;

        if (Vector3.Distance(_zombie.transform.position, _zombie.waypoints[_zombie.currentWaypointTarget].transform.position) < .5f)
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