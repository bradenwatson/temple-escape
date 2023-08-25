using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    GameObject currentItem;

    private void OnTriggerEnter(Collider collision)
    {
        if (currentItem != null) return;
        if (collision.gameObject == currentItem) return;
        if (collision.gameObject.GetComponent<SlotItem>() == null) return;

        currentItem = collision.gameObject;
        if(collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            collision.gameObject.transform.position = this.transform.position;
            collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
            collision.gameObject.transform.parent = this.transform;
            collision.gameObject.transform.localScale = new Vector3(8f, 8f, 1f);
        }
        
    }
}
