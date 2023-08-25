using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class BackwardsMovementControl : MonoBehaviour
{
    static private bool _movementIsActive = false;

    public enum ControllerType
    {
        RightHand,
        LeftHand
    }

    public Rigidbody playerBody;
    public ControllerType targetController;
    public InputActionAsset inputAction;
    public float movementSpeed = 3.0f;

    private InputAction _thumbstickInputAction;
    private InputAction _movementActive;
    private InputAction _movementCancel;

    private void Start()
    {
        playerBody.GetComponent<Rigidbody>();

        _movementActive = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Backwards Move Activate");
        _movementActive.Enable();
        _movementActive.performed += OnMovementActive;

        _movementCancel = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Backwards Move Cancel");
        _movementCancel.Enable();
        _movementCancel.performed += OnMovementCancel;

        _thumbstickInputAction = inputAction.FindActionMap("XRI " + targetController.ToString()).FindAction("Move");
        _thumbstickInputAction.Enable();
    }

    private void OnDestroy()
    {
        _movementActive.performed -= OnMovementActive;
        _movementCancel.performed -= OnMovementCancel;
    }

    private void Update()
    {
        if (!_movementIsActive)
        {
            return;
        }

        if (_thumbstickInputAction.triggered)
        {
            return;
        }

        playerBody.velocity = Vector3.back * movementSpeed;
        
        _movementIsActive = false;
    }

    private void OnMovementActive(InputAction.CallbackContext context)
    {
        if (!_movementIsActive)
        {
            _movementIsActive = true;
        }
    }

    private void OnMovementCancel(InputAction.CallbackContext context)
    {
        if (_movementIsActive)
        {
            _movementIsActive = false;
        }
    }
}
