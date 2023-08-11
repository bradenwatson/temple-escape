using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class PatrolFindingWalking : MonoBehaviour
{
    [SerializeField]
    public NavMeshAgent agent;    
    public bool isIncreasingPatrolPoints = true;
    bool moveToNewPatrolPoint = true;

    [Header("chance")]
    public float chanceOfChangingDirections = 30f;    
    float randomNumber = 0;          // have it linked to a random.range generator and if the number is less then the variable above it changes directions along the patrol route

    [Header("patrol timings")]
    public float howFarFromEachPoint = 1.5f;
    public float timeAtEachPoint = 3f;
    float howLongBeenAtCurrentPoint = 0f;    

    [Header("patrol locations")]
    public List<Transform> patrolPoints = new List<Transform>();
    public int startingLocationIndex = 0;
    int currentLocationIndex;

    [Header("sound")]    
    public float howCloseToSoundNeededToBe = 3f;
    public float howLongToWaitAtSound = 5f;
    public float howLongWaitedAtSound = 0f;
    Vector3 locationOfSound;
    bool isSoundToMoveTo = false;

    private void Start()
    {
        currentLocationIndex = startingLocationIndex;
    }

    // Update is called once per frame
    void Update()
    {        
        PatrolRoutine();        
    }

    private void PatrolRoutine()
    {
        if (!isSoundToMoveTo)
        {            
            if (moveToNewPatrolPoint)
            {
                MoveToPatrolPoint();
            }
            SeeIfAtDestination();
        }

        else
        {
            MoveToSound();
        }
    }

    private void MoveToPatrolPoint()
    {
        Vector3 patrolPointToGoTo = patrolPoints[currentLocationIndex].transform.position;
        agent.SetDestination(patrolPointToGoTo);
        moveToNewPatrolPoint = false;
    }

    private void SeeIfAtDestination()
    {
        float distance = Vector3.Distance(transform.position, patrolPoints[currentLocationIndex].position);
        if (distance < howFarFromEachPoint && howLongBeenAtCurrentPoint > timeAtEachPoint)
        {
            randomNumber = Random.Range(0, 101);
            if (randomNumber < chanceOfChangingDirections)
            {
                isIncreasingPatrolPoints = !isIncreasingPatrolPoints;
            }

            if (isIncreasingPatrolPoints)
            {
                IncreasePatrolPointIndex();
            }
            else
            {
                DecreasePatrolPointIndex();
            }

            moveToNewPatrolPoint = true;
        }

        howLongBeenAtCurrentPoint += Time.deltaTime;  
        if (distance >= howFarFromEachPoint)
        {
            howLongBeenAtCurrentPoint = 0f;
        }
    }    

    private void IncreasePatrolPointIndex()
    {
        currentLocationIndex++;
        if (currentLocationIndex >= patrolPoints.Count)
        {
            currentLocationIndex = 0;
        }
    }   

    private void DecreasePatrolPointIndex()
    {
        currentLocationIndex--;
        if (currentLocationIndex < 0)
        {
            currentLocationIndex = patrolPoints.Count - 1;
        }
    }

    private void MoveToSound()
    {
        agent.SetDestination(locationOfSound);
        SeeIfAtSoundSource();
    }

    private void SeeIfAtSoundSource()
    {       
        float distance = Vector3.Distance(transform.position, locationOfSound);
        if (distance < howCloseToSoundNeededToBe)
        {            
            if (howLongWaitedAtSound > howLongToWaitAtSound)
            {
                isSoundToMoveTo = false;
                howLongWaitedAtSound = 0;
                locationOfSound = Vector3.zero;
                MoveToPatrolPoint();
            }

            howLongWaitedAtSound += Time.deltaTime;           
        }
    }
    
    public void SetSoundTrigger(Vector3 soundLocated)
    {
        locationOfSound = soundLocated;
        isSoundToMoveTo = true;
        howLongWaitedAtSound = 0f;
    }
}
