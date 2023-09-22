using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        animator.SetBool("walking", true);
        animator.SetBool("playerSeen", false);
        animator.SetBool("closeEnoughToPlayer", false);
        brain.PlayFootSteps();
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
            animator.SetBool("walking", false);
            brain.StopFootSteps();
            timeSpentAtSound += Time.deltaTime;
        }
    }
}
