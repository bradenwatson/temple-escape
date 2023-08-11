using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [Header("Options")]
    public Vector3 endPosition;
    public float openSpeed = 0.66f; // in seconds
    public float closeDelay = 1.5f; // in seconds
    public bool startOpen = false;
    public bool testOpeningAndClosing = false;

    private bool isMoving = false;
    private bool isOpening = true;
    private Vector3 startPosition;
    private float delay = 0f;

    void Start()
    {
        startPosition = transform.position;
        if (startOpen)
        {
            transform.position = endPosition;
            isOpening = false;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            if (isOpening)
            {
                MoveDoor(endPosition);
            }
            else
            {
                MoveDoor(startPosition);
            }
        }
        else if (testOpeningAndClosing)
        {
            isMoving = true;
        }
    }

    void MoveDoor(Vector3 newPosition)
    {
        float distance = Vector3.Distance(transform.position, newPosition);
        if (distance > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, openSpeed * Time.deltaTime);
        }

        else
        {
            if (isOpening)
            {
                delay += Time.deltaTime;

                if (delay > closeDelay)
                {
                    isOpening = false;
                }
            }

            else
            {
                isMoving = false;
                isOpening = true;
            }
        }
    }

    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }
}
