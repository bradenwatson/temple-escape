using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SearchForPlayer : mBrain_base
{
    public float timeLookingForPlayer = 5f;
    public float timeLooked = 0f;
    private bool isGoingToPlayer = false;
    public Vector3 PlayersLastKnownPosition;

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
            timeLooked += Time.deltaTime;
            if (timeLooked > timeLookingForPlayer)
            {
                TransitionToNextState(searchCollectibleState);
                timeLooked = 0f;
            }
        }
    }
}
