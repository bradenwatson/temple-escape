using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class State_Patrol : mBrain_base
{
    public List<Transform> patrolPoints = new List<Transform>();
    int currentPatrolPoint = 0;
    public float minStoppingDistance = 0.1f;
    public float distanceCheckFrequency = 0.1f;
    float nextDistanceCheck = 0;
    public float waitTime = 0.3f;
    bool waiting = false;
    float nextMoveTime = 0;

    internal override void OnStateEnterArgs()
    {
        if (patrolPoints.Count > 0)
        {
            int closestPoint = 0;
            float closestDistance = Vector3.Distance(transform.position, patrolPoints[0].position);
            for (int i = 1; i < patrolPoints.Count; i++)
            {
                float currentDist = Vector3.Distance(transform.position, patrolPoints[i].position);
                if (currentDist < closestDistance)
                {
                    closestDistance = currentDist;
                    closestPoint = i;
                }
            }

            currentPatrolPoint = closestPoint;
            brain.AssignTarget(patrolPoints[currentPatrolPoint].gameObject, false);
            brain.MoveToTarget();
        }
    }

    public override void UpdateState()
    {
        PatrolRoutine();
    }

    private void PatrolRoutine()
    {
        if (brain.SeeIfPlayerIsSeen())
        {
            TransitionToNextState(attackState);
        }
        else
        {
            if (waiting && Time.time >= nextMoveTime)
            {
                waiting = false;
                GoToNextPatrolPoint();
                nextDistanceCheck = Time.time + distanceCheckFrequency;
            }
            else if (!waiting && Time.time >= nextDistanceCheck)
            {
                if (brain.GetDistanceToDestination() <= minStoppingDistance)
                {
                    waiting = true;
                    nextMoveTime += Time.time + waitTime;
                }
                else
                {
                    nextDistanceCheck = Time.time + distanceCheckFrequency;
                }
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        currentPatrolPoint++;
        if (currentPatrolPoint >= patrolPoints.Count)
        {
            currentPatrolPoint = 0;
        }
        brain.AssignTarget(patrolPoints[currentPatrolPoint].gameObject, false);
        brain.MoveToTarget();
    }
}
