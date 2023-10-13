using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameOver : MonoBehaviour
{
    [Header("Inputs")]
    public Camera mainCamera;

    [Header("Player")]
    PlayerHealth playerHealth;
    public GameObject player;

    [Header("Game Complete Menu")]
    public GameObject GameOverMenu;
    bool paused = false;

    [Header("Input Controls to Disable")]
    public GameObject leftTeleportRay;
    public GameObject rightTeleportRay;

   

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        CheckIfPlayerDied();
    }

    private void CheckIfPlayerDied()
    {
        if (playerHealth.playerIsDead == true)
        {
            Debug.Log("open Game Over Menu");
            Pause();
        }

    }

    void Pause()
    {
        paused = true;
        GameOverMenu.SetActive(true);
        Debug.Log("Set Active");
        Time.timeScale = 0f;

        leftTeleportRay.gameObject.SetActive(false);
        rightTeleportRay.gameObject.SetActive(false);

        Vector3 headPosition = mainCamera.transform.position;
        Vector3 headDirection = mainCamera.transform.forward;
        GameOverMenu.transform.position = (headPosition + headDirection * -3f)
            + new Vector3(0.75f, -1.0f, 0.0f);

        Vector3 headRotation = mainCamera.transform.eulerAngles;
        headRotation.z = 0;
        GameOverMenu.transform.eulerAngles = headRotation;
    }

    public void Resume()
    {
        paused = false;
        GameOverMenu.SetActive(false);
        Time.timeScale = 1f;

        leftTeleportRay.gameObject.SetActive(true);
        rightTeleportRay.gameObject.SetActive(true);
    }
}
