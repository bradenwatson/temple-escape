using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationControl : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider teleportationProvider;
    private InputAction _thumbstick;
    private bool _isActive = false;

    private void Start()
    {
        rayInteractor.enabled = false;

        var activate = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancel;

        _thumbstick = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        _thumbstick.Enable();
    }

    private void Update()
    {
        if (_isActive && !_thumbstick.triggered)
        {
            if (rayInteractor.GetCurrentRaycastHit(out RaycastHit hit))
            {
                TeleportRequest request = new TeleportRequest()
                {
                    destinationPosition = hit.point
                };

                teleportationProvider.QueueTeleportRequest(request);

                rayInteractor.enabled = false;
                _isActive = false;
            }
            else
            {
                rayInteractor.enabled = false;
                _isActive = false;

                return;
            }
        }
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = true;
        _isActive = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        rayInteractor.enabled = false;
        _isActive = false;
    }
}
