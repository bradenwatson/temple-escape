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

        _teleportActive = inputAction.FindActionMap("XRI " + targetController.ToString() + " Locomotion")
            .FindAction("Teleport Mode Activate");
        _teleportActive.Enable();

        _teleportCancel = inputAction.FindActionMap("XRI " + targetController.ToString() + " Locomotion")
            .FindAction("Teleport Mode Cancel");
        _teleportCancel.Enable();

        _thumbstickInputAction = inputAction.FindActionMap("XRI " + targetController.ToString() + "Locomotion")
            .FindAction("Move");
        _thumbstickInputAction.Enable();
    }
}
