using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFindingTelelport : MonoBehaviour
{
    [Header("patrol variables")]
    public List<GameObject> patrolPoints = new List<GameObject>();  // chuck in enemy game objects in unity and it will teleport to them
    public float timeSpentInBetweenPoints = 5;                      // in seconds
    public float chanceOfMonsterChangingDirection = 10;             // percent chance of changing direction while on its course
    public int startingPatrolPoint = 0;                             // is an index
    private int currentPatrolPoint = 0;                             // is an index
    private float timeSinceLastMovedPatrolPoint = 0; 
    private bool isGoingForward = true;                             // create public if you want to change the starting direction manually - the script will change the value after start

    // Start is called before the first frame update
    void Start()
    {
        currentPatrolPoint = startingPatrolPoint;
    }

    // Update is called once per frame
    void Update()
    {
        PatrolRoutine();
    }

    private void PatrolRoutine()
    {
        if (timeSinceLastMovedPatrolPoint > timeSpentInBetweenPoints)
        {
            int randomNumber = Random.Range(0, 101);
            Debug.Log(randomNumber.ToString());

            if (randomNumber < chanceOfMonsterChangingDirection)
            {
                isGoingForward != isGoingForward;
            }
            
            MovePatrolPoint(isGoingForward);
            timeSinceLastMovedPatrolPoint = 0;
        }

        // TODO: Should this be at the top of the fuinction? Otherwise we're doing the processing
        // on the frame after the condition becomes true, instead of doing it on the same frame
        // --Alex S
        timeSinceLastMovedPatrolPoint += Time.deltaTime;
    }

    private void MovePatrolPoint(bool isIncreasingPatrolPoints)
    {
        gameObject.transform.position = patrolPoints[currentPatrolPoint].transform.position;
        if (isIncreasingPatrolPoints)
        {
            currentPatrolPoint++;
            if (currentPatrolPoint >= patrolPoints.Count)
            {
                currentPatrolPoint = 0;
            }
        }
        else
        {
            currentPatrolPoint--;
            if (currentPatrolPoint < 0)
            {
                currentPatrolPoint = patrolPoints.Count - 1;
            }
        }
        
    }
}
