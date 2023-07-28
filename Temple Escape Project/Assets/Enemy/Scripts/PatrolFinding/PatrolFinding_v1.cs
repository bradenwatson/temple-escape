using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFinding_v1 : MonoBehaviour
{
    [Header("patrol variables")]
    public List<GameObject> patrolPoints = new List<GameObject>();      // chuck in enemy game objects in unity and it will teleport to them
    public float timeSpentInBetweenPoints = 5;
    public int startingPatrolPoint = 0;  // is an index
    private int currentPatrolPoint = 0;
    private float timeSinceLastMovedPatrolPoint = 0;

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
            MovePatrolPoint();
            timeSinceLastMovedPatrolPoint = 0;
        }
        timeSinceLastMovedPatrolPoint += Time.deltaTime;
    }

    private void MovePatrolPoint()
    {
        gameObject.transform.position = patrolPoints[currentPatrolPoint].transform.position;
        currentPatrolPoint++;
        if (currentPatrolPoint >= patrolPoints.Count)
        {
            currentPatrolPoint = 0;
        }
    }
}
