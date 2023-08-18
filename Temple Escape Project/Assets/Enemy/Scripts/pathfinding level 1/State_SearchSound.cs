using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class State_SearchSound : mBrain_base
{
    public bool isSearchingSound = false;
    private float lastSinceCheckedIfAtSound = 0f;
    private float rateAtCheckingIfAtSound = 0.5f;

    public Vector3 sourceToCheck;
    public float howLongToCheckSoundSource = 10f;
    public float timeAtSoundSource = 0f;

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
        if (!isSearchingSound)
        { 
            isSearchingSound = true;
            GoToSoundSource(sourceOfSoundBase);
        }
        if (brain.SeeIfPlayerIsSeen())
        {
            TransitionToNextState(attackState);
        }
        if (brain.GetDistance(sourceToCheck) < 10f)
        {
            timeAtSoundSource += Time.deltaTime;
            if (timeAtSoundSource > howLongToCheckSoundSource)
            {
                TransitionToNextState(patrolState);
                timeAtSoundSource = 0f;
            }
        }
    }

    public override void SearchSound(Vector3 sourceOfSound)
    {
        if (brain.SeeIfSeachForSound())
        {
            Debug.Log("arguemnt: " + sourceOfSound);
            //sourceOfSoundBase = sourceOfSound;
            Debug.Log("parent: " + sourceOfSoundBase);
            //TransitionToNextState(searchSoundState);
            GoToSoundSource(sourceOfSound);
        }  
    }

    public void GoToSoundSource(Vector3 sourceOfSound)
    {
        Debug.Log("go to source: " + sourceOfSound);
        brain.SetDestination(sourceOfSound);
    }
}
