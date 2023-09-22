using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SearchForPlayer : mBrain_base
{
    public float timeToLookForPlayer = 5f;
    private float timeLooked = 0f;
    private bool isGoingToPlayer = false;
    private Vector3 PlayersLastKnownPosition;

    public override void UpdateState()
    {
        LookForPlayer();
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("search state");
        timeLooked = 0f;
        if (brain.player != null)
        {
            PlayersLastKnownPosition = brain.player.transform.position;
        }
        animator.SetBool("walking", true);
        animator.SetBool("playerSeen", false);
        animator.SetBool("closeEnoughToPlayer", false);      
        brain.PlayFootSteps();
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
            if (!isGoingToPlayer)
            {
                isGoingToPlayer = true;
                brain.SetDestination(PlayersLastKnownPosition);
            }

            if (brain.GetDistance(PlayersLastKnownPosition) < 1)
            {
                animator.SetBool("walking", false);
                brain.StopFootSteps();
            }

            timeLooked += Time.deltaTime;
            if (timeLooked > timeToLookForPlayer)
            {
                TransitionToNextState(searchCollectibleState);
                timeLooked = 0f;
            }
        }
    }
}
