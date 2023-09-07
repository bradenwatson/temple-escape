using System.Collections;
using System.Collections.Generic;
using Unity.XR;
using UnityEngine;


public class InventorySlot : MonoBehaviour
{
    GameObject _currentItem;
    bool _fadeIn = false;
    bool _fadeOut = false;
    public float fadeSpeed = 0.4f;

    private void OnTriggerEnter(Collider collision)
    {
        if (_currentItem != null) return;
        if (collision.gameObject == _currentItem) return;
        if (collision.gameObject.GetComponent<SlotItem>() == null) return;

        _currentItem = collision.gameObject;
        if(collision.gameObject.GetComponent<Rigidbody>() != null && !collision.gameObject.GetComponent<SlotItem>().inInventory)
        {
            GameObject newObject = Object.Instantiate(collision.gameObject, this.transform);
            newObject.transform.position = this.transform.position;
            newObject.GetComponent<Rigidbody>().useGravity = false;
            newObject.GetComponent<SlotItem>().inInventory = true;
            newObject.transform.parent = this.transform;
            newObject.transform.localScale = new Vector3(8f, 8f, 8f);
            Object.Destroy(collision.gameObject);
        }
        
    }

    void Update() {}
    void FadeIn() {
        _fadeIn = true;
    }

}
