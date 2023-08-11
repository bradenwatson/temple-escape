using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SearchForPlayer : mBrain_base
{
    [Header("states")]
    public mBrain_base patrolState;
    public mBrain_base attackState;

    public float timeLookingForPlayer = 5f;
    public float timeLooked = 0f;

    public override void UpdateState()
    {
        LookForPlayer();
    }

    internal override void OnStateEnterArgs()
    {

    }

    private void LookForPlayer()
    {
        if (brain.distanceToAttackPlayer > brain.GetDistanceToPlayer()) 
        {
            TransitionToNextState(attackState);
            return;
        }
        timeLooked += Time.deltaTime;
        if (timeLooked > timeLookingForPlayer )
        {
            TransitionToNextState(patrolState);
            timeLooked = 0f;
        }
    }
}
