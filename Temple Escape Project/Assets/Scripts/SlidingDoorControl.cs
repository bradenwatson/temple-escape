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
    public float closeDelay = 0.5f; // in seconds

    [Tooltip("Triggers door to open if a triggering object is within this distance.\n\nSet to -1 to disable.")]
    public float enemyProximity = 0.66f;

    [Tooltip("Should this door begin in the open state\n\nOnly really useful if automatic closing is disabled (close delay < 0)")]
    public bool startOpen = false;
    //public bool testOpeningAndClosing = false;
    //public bool testClosing = false;
    //public bool testOpening = false;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isMoving = false;
    private bool isOpening = true;
    private float delay = 0f;
    private SphereCollider triggerSphere;
    private List<Collider> colliding;

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
        ProximityOnEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ProximityOnExit(other);
    }

    void Update()
    {
        if (triggerSphere.radius != enemyProximity) {
            triggerSphere.radius = Mathf.Clamp(enemyProximity, 0, float.MaxValue);
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

        //else if (testOpeningAndClosing)
        //{
        //    if (isOpening)
        //    {
        //        OpenDoor(); 
        //    }
        //    else
        //    {
        //        CloseDoor();
        //    }
        //}

        //if (testClosing)
        //{
        //    CloseDoor();
        //}

        //if (testOpening)
        //{
        //    OpenDoor();
        //}
    }

    public void ProximityOnEnter(Collider other)
    {
        if ((other.CompareTag("Enemy") && enemyProximity > 0) || other.CompareTag("Player"))
        {
            colliding.Add(other);
            OpenDoor();
        }
    }

    public void ProximityOnExit(Collider other)
    {
        // Don't need to check for proximityRadius here as the only way this will
        // trigger, is if there was an ProximityOnEnter earlier, which does the check for us
        if ((other.CompareTag("Player") || other.CompareTag("Enemy")) && colliding.Contains(other))
        {
            colliding.Remove(other);
            // Closing of door is handled in the NextState method, so that the door fully opens before closing
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
        isMoving = true;
    }

    public void CloseDoor()
    {
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
            NextState();
        }
    }

    void NextState()
    {
        if (isOpening)
        {
            if (closeDelay >= 0 && colliding.Count == 0)
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
