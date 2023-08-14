using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class State_PatrolTwo : mBrain_base
{
    [Header("patrol point positions")]
    public List<Transform> possiblePatrolPoints = new List<Transform>();
    public float distanceBufferToPatrolPoint = 5;

    [Header("timings")]
    public float timeInbetweenPoints = 5f;    
    public int currentPatrolPoint = -1; 
    public int lastPatrolPoint = -1;
    public float howCloseToPatrolPoint = 5f;
    public float timeSinceaLastChangedPoints = 0;
    public float chanceToGoBackLastPatrolPoint = 30;

    internal override void OnStateEnterArgs()
    {
        Debug.Log("patrol state");
        SetNewPatrolPoint();
    }

    public override void UpdateState()
    {
        PatrolRoutine();
    }

    private void PatrolRoutine()
    {
        if (SeeIfPlayerSeen())
        {
            TransitionToNextState(attackState);
        }
        else
        {
            if (brain.GetDistance(possiblePatrolPoints[currentPatrolPoint]) < howCloseToPatrolPoint)
            {
                timeSinceaLastChangedPoints += Time.deltaTime;
            }
            if (timeSinceaLastChangedPoints > timeInbetweenPoints)
            {
                SetNewPatrolPoint();
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
                float distanceFromPlayer = brain.GetDistance(possiblePatrolPoints[i]);
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

        float neededDistance = closestDistance + distanceBufferToPatrolPoint;
        for (int i = 0; i < possiblePatrolPoints.Count; i++)
        {
            if (brain.GetDistance(possiblePatrolPoints[i]) <= neededDistance && i != currentPatrolPoint && i != lastPatrolPoint)
            {
                closestPatrolPoints.Add(possiblePatrolPoints[i]);
            }       
        }        

        // Debug.Log($"closest patrol points count {closestPatrolPoints.Count}");
        return closestPatrolPoints;
    }

    private void GoToPatrolPoint()
    {
        brain.AssignTarget(possiblePatrolPoints[currentPatrolPoint].gameObject, false);
        brain.MoveToTarget();
    }    
}
