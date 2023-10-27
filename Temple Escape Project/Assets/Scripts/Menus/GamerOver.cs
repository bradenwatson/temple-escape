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
    public GameObject FaderScreen;
    public bool paused = false;


    [Header("Input Controls to Disable")]
    public GameObject leftTeleportRay;
    public GameObject rightTeleportRay;

   

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if(paused == false)
        {
            CheckIfPlayerDied();
        }
               
    }

    private void CheckIfPlayerDied()
    {
        if (playerHealth.playerIsDead == true)
        {
            Debug.Log("open Game Over Menu");
            FaderScreen.SetActive(true);
            Pause();
        }

    }

    void Pause()
    {
        paused = true;
        Debug.Log("in Pause(gameover) function");
        FaderScreen.SetActive(false);
        GameOverMenu.SetActive(true);
        Debug.Log("Set Active");
        Time.timeScale = 0f;

        leftTeleportRay.gameObject.SetActive(false);
        rightTeleportRay.gameObject.SetActive(false);
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
