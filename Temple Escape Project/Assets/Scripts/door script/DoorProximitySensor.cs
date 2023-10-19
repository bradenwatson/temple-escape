using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorProximitySensor : MonoBehaviour
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
        if (other.CompareTag("door_proxy_trigger") && parentDoorScript != null)
        {
            parentDoorScript.ProximityOnEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("door_proxy_trigger") && parentDoorScript != null)
        {
            parentDoorScript.ProximityOnExit(other);
        }
    }
}
