using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithDoor : MonoBehaviour
{



    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DoorTrigger"))
        {
            print("enter door");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DoorTrigger"))
        {
            print("exit door");
        }
    }
}
