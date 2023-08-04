using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFindingWalking : MonoBehaviour
{
    public int monsterSpeed = 1;
    public List<Transform> patrolPoints;

    public int startingPatrolPoint = 0;
    private int currentPatrolPoint = 0;

    public float timeStationaryEachPatrolPoint = 1;
    private float timeSinceMovedPatrolPoint = 0;

    private bool isAtPatrolPoint = false;


    private void Start()
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
        MoveToPatrolPoint();     
        if (isAtPatrolPoint)
        {
            if (timeSinceMovedPatrolPoint > timeStationaryEachPatrolPoint) 
            {
                currentPatrolPoint++;
                if (currentPatrolPoint >= patrolPoints.Count)
                {
                    currentPatrolPoint = 0;
                }
                timeSinceMovedPatrolPoint = 0;
            }
            timeSinceMovedPatrolPoint += Time.deltaTime;
            isAtPatrolPoint = false;
        }
    }

    private void MoveToPatrolPoint()
    {
        Transform target = patrolPoints[currentPatrolPoint].transform;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > 0.1f)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, target.position, monsterSpeed * Time.deltaTime);
            transform.position = newPosition;
        }
        else
        {
            isAtPatrolPoint = true;
        }
    }
}
