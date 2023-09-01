using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class State_SearchSound : mBrain_base
{
    public float howLongToCheckForSound = 20f;
    public float howLongToWaitAtSound = 3f;
    private float timeSpentAtSound = 0f;
    private float timeSpentSearching = 0f;    

    public override void UpdateState()
    {
        SearchRoutine();
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("search sound");        
    }

    private void SearchRoutine()
    {
        timeSpentSearching += Time.deltaTime;
        if (brain.SeeIfPlayerIsSeen())
        {
            TransitionToNextState(attackState);
        }
        if (timeSpentAtSound > howLongToWaitAtSound || timeSpentSearching > howLongToCheckForSound)
        {
            timeSpentAtSound = 0f;
            timeSpentSearching = 0f;
            TransitionToNextState(patrolState);
        }
        if (IsAtSound())
        {
            timeSpentAtSound += Time.deltaTime;
        }
    }
}
