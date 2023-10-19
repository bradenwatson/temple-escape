using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 3.0F;
    bool centered = false;

    void OnBecameInvisible()
    {
        centered = false;
    }

    void Update()
    {
        if (!centered)
        {
            Vector3 targetPosition = FindTargetPosition();
            MoveTowards(targetPosition);
            if (ReachedPosition(targetPosition))
            {
                centered = true;
            }
        }
    }

    private Vector3 FindTargetPosition()
    {
        return cameraTransform.position + (cameraTransform.forward * distance);
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        transform.position += (targetPosition - transform.position) * 0.025F;
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < 0.1F;
    }
}
