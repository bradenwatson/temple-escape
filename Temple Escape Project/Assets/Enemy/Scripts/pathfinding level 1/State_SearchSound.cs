using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SearchSound : mBrain_base
{
    private bool isSearchingSound = false;
    private float lastSinceCheckedIfAtSound = 0f;
    private float rateAtCheckingIfAtSound = 0.5f;

    public override void UpdateState()
    {
        SearchRoutine();
    }

    private void SearchRoutine()
    {
        if (!isSearchingSound)
        {
            GoToSoundSource();
        }
        if (SeeIfPlayerSeen())
        {
            TransitionToNextState(attackState);
        }
    }

    private void GoToSoundSource()
    {
        isSearchingSound = true;
    } 
}
