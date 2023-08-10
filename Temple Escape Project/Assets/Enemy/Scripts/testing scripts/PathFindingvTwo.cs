using System.Collections.Generic;
using UnityEngine;

public class PathFindingvTwo : MonoBehaviour
{
    public List<Transform> possiblePatrolPoints = new List<Transform>();
    public int startingPatrolPoint = 0;
    public float timeInbetweenPoints = 5f;
    float timeSinceLastPatrolPointSwitch = 0;
    int currentPatrolPoint = 0;
    int lastPatrolPoint;

    // Start is called before the first frame update
    void Start()
    {
        currentPatrolPoint = startingPatrolPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
