using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AttackPlayer : mBrain_base
{
    public override void UpdateState()
    {
        AttackRoutine();
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("attack state");
    }

    private void AttackRoutine()
    {
        brain.AssignTarget(brain.player, true);
        brain.MoveToTarget();
        if (brain.distanceToAttackPlayer <= brain.GetDistanceToPlayer())
        {
            TransitionToNextState(searchPlayerState);
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
