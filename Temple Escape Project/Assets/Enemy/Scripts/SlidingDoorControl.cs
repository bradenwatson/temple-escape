using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorControl : MonoBehaviour
{
    [Header("Options")]
    //public Vector3 endPosition;
    public Vector3 relativeEndPosition = new Vector3(0, 4.5f, 0);
    public float openSpeed = 1.5f; // in seconds
    public float closeDelay = 1.5f; // in seconds
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
