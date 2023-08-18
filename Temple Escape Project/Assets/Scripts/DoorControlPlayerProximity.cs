using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControlPlayerProximity : MonoBehaviour
{
    private DoorControl parentDoorScript;

    void Start()
    {
        parentDoorScript = GetComponentInParent<DoorControl>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player proximity on enter 1");
            parentDoorScript.ProximityOnEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player proximity on exit 1");
            parentDoorScript.ProximityOnExit(other);
        }
    }
}
