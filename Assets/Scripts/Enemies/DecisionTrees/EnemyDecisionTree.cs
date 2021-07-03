using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDecisionTree : MonoBehaviour
{
    public ZombieIA _zombie;

    QuestionNode _isPlayerInRange;
    QuestionNode _isPlayerInMelee;
    QuestionNode _isPlayerInSight;
    QuestionNode _isDie;
    
    ActionNode _actionRangeAttack;
    ActionNode _actionMeleeAttack;
    ActionNode _actionPatrol;
    ActionNode _actionSeek;
    ActionNode _actionDie;

    public INode _init;

    public EnemyDecisionTree(ZombieIA enemy)
    {
        _zombie = enemy;
    }

    public void SetNodes()
    {
        _actionMeleeAttack = new ActionNode(_zombie.ActionMeleeAttack);
        _actionRangeAttack = new ActionNode(_zombie.ActionRangeAttack);
        _actionSeek = new ActionNode(_zombie.ActionSeek);
        _actionPatrol = new ActionNode(_zombie.ActionPatrol);
        _actionDie = new ActionNode(_zombie.ActionDie);

        _isPlayerInRange = new QuestionNode(_zombie.QuestionRangeAttack, _actionRangeAttack, _actionSeek);
        _isPlayerInMelee = new QuestionNode(_zombie.QuestionMeleeAttack, _actionMeleeAttack, _actionSeek);
        _isPlayerInSight = new QuestionNode(_zombie.QuestionIsPlayerOnSight, _isPlayerInRange, _actionPatrol);
        _isDie = new QuestionNode(_zombie.HaveLifeQuestion,_actionDie, _isPlayerInSight);

        _init = _isDie;
    }

    
}
