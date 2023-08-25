using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandControl : MonoBehaviour
{
    static private bool _teleportMovementIsActive = false;

    public HandObject hand;
    private ActionBasedController controller;

    public InputActionAsset inputAction;
    private InputAction _thumbstickInputAction;

    private void Start()
    {
        controller = GetComponent<ActionBasedController>();

        _thumbstickInputAction = inputAction.FindActionMap("XRI " + hand.name.ToString()).FindAction("Thumbstick Touched");
        _thumbstickInputAction.Enable();
    }

    private async void ActivateTeleportHandMovement()
    {
        _teleportMovementIsActive = true;

        float minPosition = 0.0f, maxPosition = 1.0f;
        for (float count = minPosition; count <= maxPosition; count += 0.05f)
        {
            hand.SetTeleport(count);
            await Task.Delay(50);
        }
    }

    private async void DeactivateTeleportHandMovement()
    {
        float minPosition = 0.0f, maxPosition = 1.0f;
        for (float count = maxPosition; count >= minPosition; count -= 0.25f)
        {
            hand.SetTeleport(count);
            await Task.Delay(50);
        }

        _teleportMovementIsActive = false;
    }

    private void Update()
    {
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());

        if (_thumbstickInputAction.triggered && !_teleportMovementIsActive)
        {
            ActivateTeleportHandMovement();
        }
        
        if (!_thumbstickInputAction.triggered && _teleportMovementIsActive)
        {
            DeactivateTeleportHandMovement();
        }
    }
}