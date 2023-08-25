using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Set the relative position for the open state.\n\nExample: a y of 4.5 will move the door 4.5 up from its starting position")]
    public Vector3 relativeEndPosition = new Vector3(0, 4.5f, 0);

    [Tooltip("Time it takes for the door to go from completely closed, to completely open, and visa versa")]
    public float openSpeed = 1.5f; // in seconds

    [Tooltip("The dealy before the door automatically closes\n\nSet to -1 to disable automatic closing of the door")]
    public float closeDelay = 0.5f; // in seconds

    //[Tooltip("Comma seperated tags to ignore when cheking for proximity triggers")]
    //public string proximityBlacklist = "";

    [Tooltip("Player can trigger proximity sensor?")]
    public bool playerProxyTrigger = true;

    [Tooltip("Enemies can trigger proximity sensor?")]
    public bool enemyProxyTrigger = true;

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
    private List<Collider> colliding;
    //private List<string> blacklist;

    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(
            startPosition.x + relativeEndPosition.x,
            startPosition.y + relativeEndPosition.y,
            startPosition.z + relativeEndPosition.z);

        colliding = new List<Collider>();
        //blacklist = new List<string>();
        //foreach (string tag in proximityBlacklist.Split(","))
        //{
        //    blacklist.Add(tag.Trim());
        //}

        if (startOpen)
        {
            transform.position = endPosition;
            isOpening = false;
        }
    }

    public void ProximityOnEnter(Collider other)
    {
        //if (blacklist.Contains(other.gameObject.tag)) { return; }
        Transform otherParent = other.gameObject.transform.parent;
        if ((!playerProxyTrigger && otherParent.CompareTag("Player")) ||
            (!enemyProxyTrigger  && otherParent.CompareTag("Enemy"))) { return; }

        colliding.Add(other);
        OpenDoor();
    }

    public void ProximityOnExit(Collider other)
    {
        if (colliding.Contains(other))
        {
            colliding.Remove(other);
            // Closing of door is handled in the NextState method, so that the
            // door fully opens before closing
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
            if (closeDelay >= 0)
            {
                if (colliding.Count == 0)
                {
                    delay += Time.deltaTime;

                    if (delay > closeDelay)
                    {
                        delay = 0;
                        isOpening = false;
                    }
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

    //public bool IsMoving
    //{
    //    get { return isMoving; }
    //    set { isMoving = value; }
    //}

    //public Vector3 EndPosition
    //{
    //    get { return endPosition; }
    //    set { endPosition = value; }
    //}
}
