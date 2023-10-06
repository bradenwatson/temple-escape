using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationControl : MonoBehaviour
{
    static private bool _isTeleportActive = false;

    public enum ControllerType
    {
        LeftHand,
        RightHand
    }

    [Header("Inputs")]
    public ControllerType targetController;
    public InputActionAsset inputAction;
    public XRRayInteractor rayInteractor;
    public TeleportationProvider teleportationProvider;

    [Header("States")]
    private InputAction _thumbstickInputAction;
    private InputAction _teleportActive;
    private InputAction _teleportCancel;

    private void Start()
    {
        rayInteractor.enabled = false;

        _teleportActive = inputAction.FindActionMap(string.Format("XRI {0} Locomotion", targetController))
            .FindAction("Teleport Mode Activate");
        _teleportActive.Enable();
        _teleportActive.performed += OnTeleportActive;

        _teleportCancel = inputAction.FindActionMap(string.Format("XRI {0} Locomotion", targetController))
            .FindAction("Teleport Mode Cancel");
        _teleportCancel.Enable();
        _teleportCancel.performed += OnTeleportCancel;

        _thumbstickInputAction = inputAction.FindActionMap(string.Format("XRI {0} Locomotion", targetController))
            .FindAction("Move");
        _thumbstickInputAction.Enable();
    }

    private void OnDestroy()
    {
        _teleportActive.performed -= OnTeleportActive;
        _teleportCancel.performed -= OnTeleportCancel;
    }

    private void Update()
    {
        if (!_isTeleportActive)
        {
            return;
        }

        if (!rayInteractor.enabled)
        {
            return;
        }

        if (_thumbstickInputAction.IsPressed())
        {
            return;
        }

        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
        {
            rayInteractor.enabled = false;
            _isTeleportActive = false;

            return;
        }

        TeleportRequest teleportRequest = new TeleportRequest()
        {
            destinationPosition = raycastHit.point
        };

        teleportationProvider.QueueTeleportRequest(teleportRequest);

        rayInteractor.enabled = false;
        _isTeleportActive = false;
    }

    private void OnTeleportActive(InputAction.CallbackContext context)
    {
        if (!_isTeleportActive)
        {
            rayInteractor.enabled = true;
            _isTeleportActive = true;
        }
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        if (_isTeleportActive && rayInteractor.enabled)
        {
            rayInteractor.enabled = false;
            _isTeleportActive = false;
        }
    }
}
