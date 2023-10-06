using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseControl : MonoBehaviour
{
    [Header("Inputs")]
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
    }

    public void Resume()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
