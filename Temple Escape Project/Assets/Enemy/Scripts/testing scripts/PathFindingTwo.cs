using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PathFindingTwo : MonoBehaviour
{
    public NavMeshAgent agent;

    [Header("patrol point positions")]
    public List<Transform> possiblePatrolPoints = new List<Transform>();
    public float distanceBufferToPatrolPoint = 5;

    [Header("timings")]
    public float timeInbetweenPoints = 5f;    
    public int currentPatrolPoint = 0;
    public float howCloseToPatrolPoint = 5f;
    public float timeSinceaLastChangedPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceaLastChangedPoints = timeInbetweenPoints;
        SetNewPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        PatrolRoutine();
    }

    private void PatrolRoutine()
    {
        TimerSinceLastMovedPatrolPoints();
        if (timeSinceaLastChangedPoints > timeInbetweenPoints)
        {
            SetNewPatrolPoint();
            timeSinceaLastChangedPoints = 0;
        }        
    }

    private void TimerSinceLastMovedPatrolPoints()
    {
        if (GetDistance(possiblePatrolPoints[currentPatrolPoint]) < howCloseToPatrolPoint)
        {
            timeSinceaLastChangedPoints += Time.deltaTime;
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
                currentPatrolPoint = i;
                GoToPatrolPoint();
                break;
            }
        }
    }

    private float GetDistance(Transform positionToGetDistanceFor)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, positionToGetDistanceFor.position, NavMesh.AllAreas, path);

        float distance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return distance;
    }

    private List<Transform> GetClosestPatrolPoints()
    {
        float closestDistance = 0;
        for (int i = 0; i < possiblePatrolPoints.Count; i++)
        {
            if (i != currentPatrolPoint)
            {
                float distanceFromPlayer = GetDistance(possiblePatrolPoints[i]);
                if (distanceFromPlayer < closestDistance || closestDistance == 0)
                {
                    closestDistance = distanceFromPlayer;
                }
            }
        }

        List<Transform> closestPatrolPoints = new List<Transform>();
        float neededDistance = closestDistance + distanceBufferToPatrolPoint;
        for (int i = 0; i < possiblePatrolPoints.Count; i++) 
        {
            if (GetDistance(possiblePatrolPoints[i]) <= neededDistance)
            {
                closestPatrolPoints.Add(possiblePatrolPoints[i]);
            }
        }
        return closestPatrolPoints;
    }

    private void GoToPatrolPoint()
    {
        agent.SetDestination(possiblePatrolPoints[currentPatrolPoint].position);
    }
}
