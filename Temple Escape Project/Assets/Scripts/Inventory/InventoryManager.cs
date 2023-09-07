using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    public InputActionReference secondaryButtonInputActionRef;

    private bool _inventoryVisible = false;


    private void Update()
    {
        if(SecondaryButtonPressed())
        {
            Debug.Log("Secondary pressed!");
            if (!_inventoryVisible)
            {
                ToggleInventory(true);
            }
        } else
        {
            if(_inventoryVisible)
            {
                ToggleInventory(false);
            }
        }
    }

    //Toggles the inventory to be seen if supplied true, hides if false.
    private void ToggleInventory(bool visible)
    {
        foreach(Transform child in transform) {
            child.gameObject.SetActive(visible);
        }
        _inventoryVisible = visible;
    }
    //Returns true if the secondary button is being pressed.
    private bool SecondaryButtonPressed() { return secondaryButtonInputActionRef.action.ReadValue<float>() == 1; }

    public void ItemPickup(SelectEnterEventArgs args) {
        Debug.Log("Fired item pickup! Item: " + args.interactableObject.ToString());    
        GameObject item = args.interactableObject.transform.gameObject;
        SlotItem slotItem= item.GetComponent<SlotItem>();
        if(slotItem.inInventory) { return; }
    }
    public void ItemDrop(SelectExitEventArgs args) {
        Debug.Log("Fired item pickup! Item: " + args.interactableObject.ToString());    
        GameObject item = args.interactableObject.transform.gameObject;
        SlotItem slotItem= item.GetComponent<SlotItem>();
        if(slotItem.inInventory) { 
            Rigidbody body = item.GetComponent<Rigidbody>();
            body.isKinematic = false;
            body.useGravity = true;
            item.transform.SetParent(null);
            item.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            slotItem.inInventory = false;
         }
    }
}
