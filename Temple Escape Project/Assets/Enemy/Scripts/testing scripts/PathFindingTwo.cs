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
    public int currentPatrolPoint = -1; 
    public int lastPatrolPoint = -1;
    public float howCloseToPatrolPoint = 5f;
    public float timeSinceaLastChangedPoints = 0;
    public float chanceToGoBackLastPatrolPoint = 30;

    // Start is called before the first frame update
    void Start()
    {
        SetNewPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        PatrolRoutine();
    }

    private void PatrolRoutine()
    {
        if (GetDistance(possiblePatrolPoints[currentPatrolPoint]) < howCloseToPatrolPoint)
        {
            timeSinceaLastChangedPoints += Time.deltaTime;
        }
        if (timeSinceaLastChangedPoints > timeInbetweenPoints)
        {
            SetNewPatrolPoint();
            timeSinceaLastChangedPoints = 0;
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
            if (i != currentPatrolPoint && i != lastPatrolPoint)
            {
                float distanceFromPlayer = GetDistance(possiblePatrolPoints[i]);
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
            Debug.Log(randomNumber);
            if (randomNumber < chanceToGoBackLastPatrolPoint)
            {
                closestPatrolPoints.Add(possiblePatrolPoints[lastPatrolPoint]);
            }
        }

        float neededDistance = closestDistance + distanceBufferToPatrolPoint;
        for (int i = 0; i < possiblePatrolPoints.Count; i++)
        {
            if (GetDistance(possiblePatrolPoints[i]) <= neededDistance && i != currentPatrolPoint && i != lastPatrolPoint)
            {
                closestPatrolPoints.Add(possiblePatrolPoints[i]);
            }       
        }        

        Debug.Log($"closest patrol points count {closestPatrolPoints.Count}");
        return closestPatrolPoints;
    }

    private void GoToPatrolPoint()
    {
        agent.SetDestination(possiblePatrolPoints[currentPatrolPoint].position);
    }
}
