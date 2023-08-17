using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorControl : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Set the relative position for the open state.\n\nExample: a y of 4.5 will move the door 4.5 up from its starting position.")]
    public Vector3 relativeEndPosition = new Vector3(0, 4.5f, 0);

    [Tooltip("Time it takes for the door to go from completely closed, to completely open, and visa versa")]
    public float openSpeed = 1.5f; // in seconds

    [Tooltip("The dealy before the door automatically closes.\n\nSet to -1 to disable automatic closing of the door.")]
    public float closeDelay = 1.5f; // in seconds

    [Tooltip("Triggers door to open if a triggering object is within this distance.\n\nSet to -1 to disable.")]
    public float proximityRadius = -1f;

    [Tooltip("Should this door begin in the open state")]
    public bool startOpen = false;
    public bool testOpeningAndClosing = false;
    public bool testClosing = false;
    public bool testOpening = false;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isMoving = false;
    private bool isOpening = true;
    private float delay = 0f;
    private SphereCollider triggerSphere;
    private List<Collider> colliding;
    private bool closeWhenReady = false;
    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(
            startPosition.x + relativeEndPosition.x,
            startPosition.y + relativeEndPosition.y,
            startPosition.z + relativeEndPosition.z);

        triggerSphere = GetComponent<SphereCollider>();
        colliding = new List<Collider>();

        if (startOpen)
        {
            transform.position = endPosition;
            isOpening = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            colliding.Add(other);
            OpenDoor();
            closeWhenReady = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && colliding.Contains(other))
        {
            colliding.Remove(other);
            if (colliding.Count <= 0)
            {
                CloseDoor();
            }
        }
    }

    void Update()
    {
        if (proximityRadius > 0f && triggerSphere.radius != proximityRadius) {
            triggerSphere.radius = proximityRadius;
        }

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
            if (isOpening)
            {
                OpenDoor(); 
            }
            else
            {
                CloseDoor();
            }
        }

        if (testClosing)
        {
            CloseDoor();
        }

        if (testOpening)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
        isMoving = true;
    }

    public void CloseDoor(bool waitUntilOpen = true)
    {
        //if (waitUntilOpen && isMoving) {
        //    TODO: make door wait until it's fully open before closing
        //}

        isOpening = false;
        isMoving = true;
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
            nextState();
        }
    }

    void nextState()
    {
        if (isOpening)
        {
            // TODO: Hold door open if trigger still active
            if (closeDelay >= 0)
            {
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
                isOpening = false;
            }
        }

        else
        {
            isMoving = false;
            isOpening = true;
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
