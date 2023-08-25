using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public InputActionReference secondaryButtonInputActionRef;

    private bool _inventoryVisible = false;


    private void Update()
    {
        if(SecondaryButtonPressed())
        {
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
        this.gameObject.SetActive(visible);
        _inventoryVisible = visible;
    }
    //Returns true if the secondary button is being pressed.
    private bool SecondaryButtonPressed() { return secondaryButtonInputActionRef.action.ReadValue<int>() == 1; }
}
