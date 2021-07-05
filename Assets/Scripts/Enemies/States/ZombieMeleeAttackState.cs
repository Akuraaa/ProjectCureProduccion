using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMeleeAttackState : ZombieState
{
    Transform target;
    public ZombieMeleeAttackState(StateMachine sm, ZombieIA zombie) : base(sm, zombie)
    {
    }

    public override void Awake()
    {
        base.Awake();
        _zombie.animator.SetTrigger("MeleeAttack");
        _zombie.speed = 0;
    }

    public override void Execute()
    {
        base.Execute();
        target = _zombie.player.transform;
        //_zombie.direction = new Vector3(target.position.x, _zombie.transform.position.y, target.position.z);
        //_zombie.rotGoal = Quaternion.LookRotation(_zombie.direction);
        //_zombie.transform.rotation = Quaternion.Slerp(_zombie.transform.rotation, _zombie.rotGoal, _zombie.turnSpeed);
        _zombie.direction = new Vector3(target.position.x, _zombie.transform.position.y, target.position.z);
        _zombie.transform.rotation = Quaternion.LookRotation(_zombie.direction - _zombie.transform.position);
        _zombie.transform.position += _zombie.transform.forward * _zombie.speed * Time.deltaTime;
        //_zombie.transform.LookAt(_zombie.player.transform);
    }
}