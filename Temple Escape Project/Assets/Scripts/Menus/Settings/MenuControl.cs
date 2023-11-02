using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject instructionsMenu;

    // Set mainMenu to active and the rest to inactive.
    private void Start()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        instructionsMenu.SetActive(false);
    }

    // Load scene by index value.
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    
    // Exit application when exit button is pressed.
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }
}
