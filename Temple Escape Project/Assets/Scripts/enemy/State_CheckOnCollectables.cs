using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class State_CheckOnCollectables : mBrain_base
{
    public List<GameObject> collectables = new List<GameObject>();
    public float distanceStopFromTarget = 1f;        
    public float timeToStayAtEachCollectable = 1f;
    private int currentIndex = 0;

    bool goingToCollectable = false;    

    public override void UpdateState()
    {
        CheckCollectablesRoutine();
    }

    internal override void OnStateEnterArgs()
    {
        currentIndex = 0;

        animator.SetBool("walking", true);
        animator.SetBool("playerSeen", false);
        animator.SetBool("closeEnoughToPlayer", false);

        brain.PlayFootSteps();
    }

    private void CheckCollectablesRoutine()
    {       
        if (brain.SeeIfPlayerIsSeen())
        {
            TransitionToNextState(attackState);
        }
        if (SeeIfPieceMissing())
        {
            brain.MonsterSpeed(isCollectableInfluenced:true);
        }       
    }

    private bool SeeIfPieceMissing()
    {       
        bool pieceMissing = false;

        if (currentIndex < 0 || currentIndex >= collectables.Count)
        {
            TransitionToNextState(patrolState);
            return false;
        }

        PuzzleInformation puzzleInformation = collectables[currentIndex].GetComponent<PuzzleInformation>();

        if (puzzleInformation == null)
        {
            print("there is no puzzle information on the puzzle piece");
            return false;
        }

        if (!goingToCollectable)
        {
            GoToCollectable(puzzleInformation);
            goingToCollectable = true;
        }

        float distance = brain.GetDistance(puzzleInformation.startLocation);

        if (distance < distanceStopFromTarget)
        {
            if (puzzleInformation.moved)
            {
                pieceMissing = true;
            }
            goingToCollectable = false;
            currentIndex += 1;
        }

        return pieceMissing;
    }

    private void GoToCollectable(PuzzleInformation puzzleInformation)
    {
        brain.SetDestination(puzzleInformation.startLocation);
    }
}
