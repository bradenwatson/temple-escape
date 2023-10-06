using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
