using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameComplete1 : MonoBehaviour
{
    [Header("Inputs")]
    public Camera mainCamera;

    [Header("Game Complete Menu")]
    public GameObject GameCompleteMenu;
    bool paused = false;

    [Header("Input Controls to Disable")]
    public GameObject leftTeleportRay;
    public GameObject rightTeleportRay;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("collision entered");
            Pause();
            
        }
        
    }

    void Pause()
    {
        paused = true;
        GameCompleteMenu.SetActive(true);
        Debug.Log("Set Active");
        Time.timeScale = 0f;

        leftTeleportRay.gameObject.SetActive(false);
        rightTeleportRay.gameObject.SetActive(false);

        Vector3 headPosition = mainCamera.transform.position;
        Vector3 headDirection = mainCamera.transform.forward;
        GameCompleteMenu.transform.position = (headPosition + headDirection * -3f)
            + new Vector3(0.75f, -1.0f, 0.0f);

        Vector3 headRotation = mainCamera.transform.eulerAngles;
        headRotation.z = 0;
        GameCompleteMenu.transform.eulerAngles = headRotation;
    }

    public void Resume()
    {
        paused = false;
        GameCompleteMenu.SetActive(false);
        Time.timeScale = 1f;

        leftTeleportRay.gameObject.SetActive(true);
        rightTeleportRay.gameObject.SetActive(true);
    }
}
