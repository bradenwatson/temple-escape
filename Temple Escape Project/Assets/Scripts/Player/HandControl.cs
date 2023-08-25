using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandControl : MonoBehaviour
{
    public HandObject hand;
    private ActionBasedController controller;

    private void Start()
    {
        controller = GetComponent<ActionBasedController>();
    }

    public async void ActivateTeleportHandMovement()
    {
        float minPosition = 0.0f, maxPosition = 1.0f;
        for (float count = minPosition; count <= maxPosition; count += 0.25f)
        {
            hand.SetTeleport(count);
            await Task.Delay(25);
        }

        await Task.Delay(50);

        hand.SetTeleport(minPosition);
    }

    private void Update()
    {
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }
}