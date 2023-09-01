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

    private float timeAtCollectable = 0f;
    private bool goingToCollectable = false;
    private int totalCollectablesFound = 0;

    private List<int> randomInts = new List<int>();     
    private List<Vector3> collectablesTransform = new List<Vector3>();

    public override void UpdateState()
    {
        CheckCollectablesRoutine();
    }

    internal override void OnStateEnterArgs()
    {
        Debug.Log("collectable state");
        totalCollectablesFound = 0;
        timeAtCollectable = 0;
        currentIndex = 0;
    }

    private void Start()
    {
        GetTransformPositionOfCollectables();
    }

    private void CheckCollectablesRoutine()
    {
        if (collectables.Count == 0)
        {
            print(42);
            TransitionToNextState(patrolState);
            return;
        }        
        if (brain.SeeIfPlayerIsSeen())
        {
            print(48);
            TransitionToNextState(attackState);
        }
        if (SeeIfPieceMissing())
        {
            brain.MonsterSpeed(true, false, false, false);
        }       
    }

    private bool SeeIfPieceMissing()
    {
        bool whatToReturn = false;
        if (randomInts.Count == 0)
        {
            while (randomInts.Count < collectables.Count)
            {
                int rnd = Random.Range(0, collectables.Count);             
                if (!randomInts.Contains(rnd))
                {
                    randomInts.Add(rnd);
                }
            }
        }
        if (!goingToCollectable)
        {
            GoToCollectable();
        }
        if (brain.GetDistance(collectablesTransform[randomInts[currentIndex]]) < distanceStopFromTarget)
        {            
            timeAtCollectable += Time.deltaTime;
            if (timeAtCollectable > timeToStayAtEachCollectable)
            {  
                if (brain.GetDistance(collectables[randomInts[currentIndex]].transform.position) > distanceStopFromTarget)
                {
                    Debug.Log("missing collectable" + currentIndex);
                    whatToReturn = true;
                }           
                goingToCollectable = false;
                totalCollectablesFound++;
                currentIndex++;
                if (currentIndex >= collectables.Count)
                {
                    print(90);
                    TransitionToNextState(patrolState);
                }
                timeAtCollectable = 0;            
            }            
        }
        return whatToReturn;
    }

    private void GoToCollectable()
    {
        goingToCollectable = true;        
        if (currentIndex < collectables.Count)
        {
            brain.SetDestination(collectablesTransform[randomInts[currentIndex]]);            
        }             
        if (currentIndex >= collectables.Count)
        {
            currentIndex = collectables.Count - 1;
        }
    }

    private void GetTransformPositionOfCollectables()
    {
        for (int i = 0; i < collectables.Count; i++)
        {
            collectablesTransform.Add(collectables[i].transform.position);
        }
    }
}
