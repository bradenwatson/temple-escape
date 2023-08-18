using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class DoorControl : MonoBehaviour
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

    [Tooltip("Whether or not the player should triger the proximity sensor.")]
    public bool playerProximityTrigger = true;

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
    private SphereCollider enemyTriggerSphere;
    private List<Collider> colliding;

    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(
            startPosition.x + relativeEndPosition.x,
            startPosition.y + relativeEndPosition.y,
            startPosition.z + relativeEndPosition.z);

        enemyTriggerSphere = gameObject.AddComponent<SphereCollider>();
        enemyTriggerSphere.center = Vector3.zero;
        enemyTriggerSphere.isTrigger = true;

        colliding = new List<Collider>();

        if (startOpen)
        {
            transform.position = endPosition;
            isOpening = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy on enter 1");
            ProximityOnEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy on exit 1");
            ProximityOnExit(other);
        }
    }

    public void ProximityOnEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player proximity on enter 2");
        }

        if ((other.CompareTag("Enemy") && enemyProximity > 0)
            || (other.CompareTag("Player") && playerProximityTrigger))
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player proximity on enter 3");
            }
            colliding.Add(other);
            OpenDoor();
        }
    }

    public void ProximityOnExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player proximity on exit 2");
        }
        // Don't need to check for proximityRadius here as the only way this will
        // trigger, is if there was an ProximityOnEnter earlier, which does the check for us
        if ((other.CompareTag("Player") || other.CompareTag("Enemy"))
            && colliding.Contains(other))
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player proximity on exit 3");
            }
            colliding.Remove(other);
            Debug.Log(colliding.Count);
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

    void Update()
    {
        if (enemyTriggerSphere.radius != enemyProximity)
        {
            enemyTriggerSphere.radius = Mathf.Clamp(enemyProximity, 0, float.MaxValue);
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
            Debug.Log("Checking closing of door");
            Debug.Log(colliding.Count);
            Debug.Log(colliding.Count == 0);
            foreach (var collider in colliding)
            {
                Debug.Log(collider.gameObject.name);
            }
            Debug.Log(closeDelay >= 0 && colliding.Count == 0);
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
