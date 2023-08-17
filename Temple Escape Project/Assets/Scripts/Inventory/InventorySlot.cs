using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    GameObject currentItem;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colliding!");
        if (currentItem != null) return;
        Debug.Log("Colliding23!");
        if (collision.gameObject == currentItem) return;
        Debug.Log("Colliding123!");
        if (collision.gameObject.GetComponent<SlotItem>() == null) return;
        Debug.Log("Colliding!2");

        currentItem = collision.gameObject;
        if(collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Debug.Log("Colliding3!");
            collision.gameObject.transform.position = this.transform.position;
            collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
        
    }
}
