using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_CheckOnCollectables : mBrain_base
{
    public List<GameObject> collectables = new List<GameObject>();

    public override void UpdateState()
    {
        CheckCollectablesRoutine();
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("collectable routine");
    }

    private void CheckCollectablesRoutine()
    {
        if (SeeIfPieceMissing())
        {
            brain.MonsterSpeed(SeeIfPieceMissing(), false, false, false);
        }
        TransitionToNextState(patrolState);
    }

    private bool SeeIfPieceMissing()
    {   // needs code to navigate towards each piece and see if they are missing
        return true;
    }
}
