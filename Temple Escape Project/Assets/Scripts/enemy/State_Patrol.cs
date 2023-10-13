using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;

public class State_Patrol : mBrain_base
{
    [Header("patrol points")]
    public List<Transform> patrolPoints = new List<Transform>();
    public GameObject patrolPointGroupings;
    int currentPatrolPoint = 0;

    [Header("timings")]
    public float defaultTimeAtPatrolPoint = 2;
    private float patrolPointTime = -1;
    private float timeSinceAtPatrolPoint = 0f;

    [Header("other")]
    public float distanceFromPatrolPoint = 1f;

    void AddEachChildElementFromGameObjectToPatrolPoints()
    {
        foreach (Transform gameObject in patrolPointGroupings.transform)
        {
            if (!patrolPoints.Contains(gameObject))
            {
                patrolPoints.Add(gameObject);
            }
        }
    }

    internal override void OnStateEnterArgs()
    {
        AddEachChildElementFromGameObjectToPatrolPoints();

        animator.SetBool("walking", true);
        animator.SetBool("playerSeen", false);
        animator.SetBool("closeEnoughToPlayer", false);
        brain.PlayFootSteps();
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
                animator.SetBool("walking", false);
                brain.StopFootSteps();
                if (patrolPointTime == -1)
                {
                    patrolPointTime = patrolPoints[currentPatrolPoint].GetComponent<PatrolPoint>().timeToStay();
                }
                if (patrolPointTime == -1)
                {
                    patrolPointTime = defaultTimeAtPatrolPoint;
                }
                timeSinceAtPatrolPoint += Time.deltaTime;
                if (timeSinceAtPatrolPoint > patrolPointTime)
                {
                    patrolPointTime = -1;
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
        animator.SetBool("walking", true);
        brain.PlayFootSteps();
    }
}
