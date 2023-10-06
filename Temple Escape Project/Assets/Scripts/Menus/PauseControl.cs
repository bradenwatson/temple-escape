using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseControl : MonoBehaviour
{
    [Header("Inputs")]
    public Camera mainCamera;
    public InputActionAsset inputAction;

    [Header("State")]
    private InputAction _menuButtonInputAction;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    bool paused = false;

    private void Start()
    {
        _menuButtonInputAction = inputAction.FindActionMap(string.Format("XRI LeftHand Interaction"))
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
            }
        }
    }

    void Pause()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        Vector3 headPosition = mainCamera.transform.position;
        Vector3 headDirection = mainCamera.transform.forward;
        pauseMenu.transform.position = (headPosition + headDirection * -3f)
            + new Vector3(0.75f, -1.0f, 0.0f);

        Vector3 headRotation = mainCamera.transform.eulerAngles;
        headRotation.z = 0;
        pauseMenu.transform.eulerAngles = headRotation; 
    }

    public void Resume()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
