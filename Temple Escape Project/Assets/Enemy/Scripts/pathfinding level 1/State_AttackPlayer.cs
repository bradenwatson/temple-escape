using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AttackPlayer : mBrain_base
{
    public mBrain_base searchForTarget;

    public override void UpdateState()
    {
        AttackRoutine();
    }

    internal override void OnStateEnterArgs()
    {
        
    }

    private void AttackRoutine()
    {
        brain.AssignTarget(brain.player, true);
        brain.MoveToTarget();
        if (brain.distanceToAttackPlayer <= brain.GetDistanceToPlayer())
        {
            TransitionToNextState(searchForTarget);
        }
        else
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        
    }
}
