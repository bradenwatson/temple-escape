using UnityEngine;
using UnityEngine.InputSystem;

public class PauseControl : MonoBehaviour
{
    public enum ControllerType
    {
        LeftHand,
        RightHand
    }

    [Header("Inputs")]
    public ControllerType targetController;
    public InputActionAsset inputAction;

    [Header("State")]
    private InputAction _menuButtonInputAction;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    bool paused = false;

    [Header("GameObjects to Disable")]
    public GameObject levelDisplay;
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
            }
        }
    }

    void Pause()
    {
        levelDisplay.SetActive(false);

        paused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        leftTeleportRay.gameObject.SetActive(false);
        rightTeleportRay.gameObject.SetActive(false);
    }

    public void Resume()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        leftTeleportRay.gameObject.SetActive(true);
        rightTeleportRay.gameObject.SetActive(true);
    }
}
