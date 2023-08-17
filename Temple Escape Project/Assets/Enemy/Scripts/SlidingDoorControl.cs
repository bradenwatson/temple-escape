using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorControl : MonoBehaviour
{
    [Header("Options")]
    //public Vector3 endPosition;
    [Tooltip("Set the relative position for the open state.\n\nExample: a y of 4.5 will move the door 4.5 up from its starting position.")]
    public Vector3 relativeEndPosition = new Vector3(0, 4.5f, 0);
    [Tooltip("Time it takes for the door to go from completely closed, to completely open, and visa versa")]
    public float openSpeed = 1.5f; // in seconds
    [Tooltip("The dealy before the door automatically closes.\n\nSet to -1 to disable automatic closing of the door.")]
    public float closeDelay = 1.5f; // in seconds
    [Tooltip("Triggers door to open if a triggering object is within this distance.\n\nSet to -1 to disable.")]
    public float proximityTrigger = -1f;
    [Tooltip("Should this door begin in the open state")]
    public bool startOpen = false;
    public bool testOpeningAndClosing = false;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isMoving = false;
    private bool isOpening = true;
    private float delay = 0f;

    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(
            startPosition.x + relativeEndPosition.x,
            startPosition.y + relativeEndPosition.y,
            startPosition.z + relativeEndPosition.z);

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
                // TODO: Hold door open if trigger still active
                delay += Time.deltaTime;

                if (delay > closeDelay)
                {
                    delay = 0;
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

    public Vector3 EndPosition
    {
        get { return endPosition; }
        set {  endPosition = value; }
    }
}
