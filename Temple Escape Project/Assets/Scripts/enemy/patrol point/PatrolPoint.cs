using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public float minTimeToStayAtPatrolPoint = 1f;
    public float maxTimeToStayAtPatrolPoint = 3f;

    public float timeToStay()
    {
        return Random.Range(minTimeToStayAtPatrolPoint, maxTimeToStayAtPatrolPoint);
    }
}
