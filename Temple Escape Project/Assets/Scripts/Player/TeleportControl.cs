using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportControl : MonoBehaviour
{
    static private bool _teleportIsActive = false;

    public enum ControllerType
    {
        RightHand,
        LeftHand
    }

    public HandControl handControl;

    public ControllerType targetController;
    public InputActionAsset inputAction;
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;

    private InputAction _thumbstickInputAction;
    private InputAction _teleportActive;
    private InputAction _teleportCancel;

    private void Start()
    {
        rayInteractor.enabled = false;

        _teleportActive = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Teleport Mode Activate");
        _teleportActive.Enable();
        _teleportActive.performed += OnTeleportActive;

        _teleportCancel = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Teleport Mode Cancel");
        _teleportCancel.Enable();
        _teleportCancel.performed += OnTeleportCancel;

        _thumbstickInputAction = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Move");
        _thumbstickInputAction.Enable();
    }

    private void OnDestroy()
    {
        _teleportActive.performed -= OnTeleportActive;
        _teleportCancel.performed -= OnTeleportCancel;
    }

    private void Update()
    {
        if (!_teleportIsActive)
        {
            return;
        }

        if (!rayInteractor.enabled)
        {
            return;
        }

        if (_thumbstickInputAction.triggered)
        {
            return;
        }

        handControl.ActivateTeleportHandMovement();

        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
        {
            rayInteractor.enabled = false;
            _teleportIsActive = false;

            return;
        }

        TeleportRequest teleportRequest = new TeleportRequest()
        {
            destinationPosition = raycastHit.point
        };

        teleportationProvider.QueueTeleportRequest(teleportRequest);

        rayInteractor.enabled = false;
        _teleportIsActive = false;
    }

    private void OnTeleportActive(InputAction.CallbackContext context)
    {
        if (!_teleportIsActive)
        {
            rayInteractor.enabled = true;
            _teleportIsActive = true;
        }
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        if (_teleportIsActive && rayInteractor.enabled)
        {
            rayInteractor.enabled = false;
            _teleportIsActive = false;
        }
    }
}
