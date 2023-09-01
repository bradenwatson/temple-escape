using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class State_Patrol : mBrain_base
{
    [Header("patrol points")]
    public List<Transform> patrolPoints = new List<Transform>();
    int currentPatrolPoint = 0;

    [Header("timings")]
    public float minTimeToWaitAtPatrolPoint = 1f;
    public float maxTimeToWaitAtPatrolPoint = 3f;
    private float timeToWaitAtPatrolPoint = 0f;
    private float timeSinceAtPatrolPoint = 0f;

    [Header("other")]
    public float distanceFromPatrolPoint = 1f;


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
            if (brain.GetDistance(patrolPoints[currentPatrolPoint].position) < distanceFromPatrolPoint)
            {
                if (timeToWaitAtPatrolPoint <= 0f)
                {
                    timeToWaitAtPatrolPoint = Random.Range(minTimeToWaitAtPatrolPoint, maxTimeToWaitAtPatrolPoint);
                }
                timeSinceAtPatrolPoint += Time.deltaTime;
                if (timeSinceAtPatrolPoint > timeToWaitAtPatrolPoint)
                {
                    timeToWaitAtPatrolPoint = 0f;
                    timeSinceAtPatrolPoint = 0f;
                    GoToNextPatrolPoint();
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
