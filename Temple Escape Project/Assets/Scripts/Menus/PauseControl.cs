using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD

public class PauseControl : MonoBehaviour
{
    public GameObject pauseMenu;
    bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                Resume();
=======
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseControl : MonoBehaviour
{
    public enum ControllerType
    {
        LeftHand,
        RightHand
    }

    [Header("Inputs")]
    public Camera mainCamera;
    public ControllerType targetController;
    public InputActionAsset inputAction;

    [Header("State")]
    private InputAction _menuButtonInputAction;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    bool paused = false;

    [Header("Input Controls to Disable")]
    public GameObject leftTeleportRay;
    public GameObject rightTeleportRay;

    private void Start()
    {
        _menuButtonInputAction = inputAction.FindActionMap(string.Format("XRI {0} Interaction", targetController))
            .FindAction("Pause Press");
        _menuButtonInputAction.Enable();
    }

    void Update()
    {
        if (_menuButtonInputAction.triggered)
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
>>>>>>> 4d4016b46a62a115229e2e0d3e8b94da4959829d
            }
        }
    }

    void Pause()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
<<<<<<< HEAD
=======

        leftTeleportRay.gameObject.SetActive(false);
        rightTeleportRay.gameObject.SetActive(false);

        Vector3 headPosition = mainCamera.transform.position;
        Vector3 headDirection = mainCamera.transform.forward;
        pauseMenu.transform.position = (headPosition + headDirection * -3f)
            + new Vector3(0.75f, -1.0f, 0.0f);

        Vector3 headRotation = mainCamera.transform.eulerAngles;
        headRotation.z = 0;
        pauseMenu.transform.eulerAngles = headRotation; 
>>>>>>> 4d4016b46a62a115229e2e0d3e8b94da4959829d
    }

    public void Resume()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
<<<<<<< HEAD
=======

        leftTeleportRay.gameObject.SetActive(true);
        rightTeleportRay.gameObject.SetActive(true);
>>>>>>> 4d4016b46a62a115229e2e0d3e8b94da4959829d
    }
}
