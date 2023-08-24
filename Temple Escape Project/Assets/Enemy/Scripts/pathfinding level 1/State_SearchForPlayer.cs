using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SearchForPlayer : mBrain_base
{
    public float timeLookingForPlayer = 5f;
    public float timeLooked = 0f;

    public override void UpdateState()
    {
        LookForPlayer();
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("search state");
        timeLooked = 0f;
    }

    private void LookForPlayer()
    {
        if (brain.SeeIfPlayerIsSeen())
        {
            TransitionToNextState(attackState);
            return;
        }
        else
        {
            timeLooked += Time.deltaTime;
            if (timeLooked > timeLookingForPlayer)
            {
                TransitionToNextState(searchCollectibleState);
                timeLooked = 0f;
            }
        }
    }
}
