using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class State_PatrolTwo : mBrain_base
{
    [Header("patrol point positions")]
    public List<Transform> possiblePatrolPoints = new List<Transform>();
    public float distanceBufferChosingPatrolPoint = 5f;

    [Header("timings")]
    public float defaultTimeInbetweenPoints = 5f;   
    public float howCloseToPatrolPoint = 1f;    
    public float chanceToGoBackLastPatrolPoint = 30;
    private float timeSinceaLastChangedPoints = 0; 
    private int currentPatrolPoint = -1; 
    private int lastPatrolPoint = -1;
    private float patrolPointTime = -1;

    internal override void OnStateEnterArgs()
    {
        Debug.Log("patrol state");
        SetNewPatrolPoint(); 
        lastPatrolPoint = currentPatrolPoint;
        animator.SetBool("walking", true);
        animator.SetBool("playerSeen", false);
        animator.SetBool("closeEnoughToPlayer", false);
        brain.PlayFootSteps();
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
            if (possiblePatrolPoints.Count == 0)
            {
                return;
            }
            if (patrolPointTime == -1)
            {
                patrolPointTime = possiblePatrolPoints[currentPatrolPoint].GetComponent<PatrolPoint>().timeToStay();
            }
            if (patrolPointTime == -1)
            {
                patrolPointTime = defaultTimeInbetweenPoints;
            }
            if (brain.GetDistance(possiblePatrolPoints[currentPatrolPoint].position) < howCloseToPatrolPoint)
            {
                animator.SetBool("walking", false);
                timeSinceaLastChangedPoints += Time.deltaTime;
            }
            if (timeSinceaLastChangedPoints > patrolPointTime)
            {
                animator.SetBool("walking", true);
                SetNewPatrolPoint();
                patrolPointTime = -1f;
                timeSinceaLastChangedPoints = 0;
            }
        }
    }

    private void SetNewPatrolPoint()
    {        
        List<Transform> potentialPatrolPoints = GetClosestPatrolPoints();
        Transform newPatrolPoint = potentialPatrolPoints[Random.Range(0, potentialPatrolPoints.Count)];
        for (int i = 0; i < possiblePatrolPoints.Count; i++)
        {
            if (possiblePatrolPoints[i] == newPatrolPoint)
            {
                lastPatrolPoint = currentPatrolPoint;
                currentPatrolPoint = i;              
                GoToPatrolPoint();
                break;
            }
        }
    }

    private List<Transform> GetClosestPatrolPoints()
    {
        float closestDistance = 0;
        for (int i = 0; i < possiblePatrolPoints.Count; i++)
        {
            if (i != currentPatrolPoint && i != lastPatrolPoint)
            {
                float distanceFromPlayer = brain.GetDistance(possiblePatrolPoints[i].position);
                if (distanceFromPlayer < closestDistance || closestDistance == 0)
                {
                    closestDistance = distanceFromPlayer;
                }
            }
        }

        List<Transform> closestPatrolPoints = new List<Transform>();
        if (lastPatrolPoint != -1)
        {
            int randomNumber = Random.Range(0, 101);
            if (randomNumber < chanceToGoBackLastPatrolPoint)
            {
                closestPatrolPoints.Add(possiblePatrolPoints[lastPatrolPoint]);
            }
        }

        float neededDistance = closestDistance + distanceBufferChosingPatrolPoint;
        for (int i = 0; i < possiblePatrolPoints.Count; i++)
        {
            if (brain.GetDistance(possiblePatrolPoints[i].position) <= neededDistance && i != currentPatrolPoint && i != lastPatrolPoint)
            {
                closestPatrolPoints.Add(possiblePatrolPoints[i]);
            }       
        }        

        // Debug.Log($"closest patrol points count {closestPatrolPoints.Count}");

        if (closestPatrolPoints.Count == 0)
        {
            closestPatrolPoints.Add(possiblePatrolPoints[lastPatrolPoint]);
        }

        return closestPatrolPoints;
    }

    private void GoToPatrolPoint()
    {
        brain.AssignTarget(possiblePatrolPoints[currentPatrolPoint].gameObject, false);
        brain.MoveToTarget();
    }    
}
