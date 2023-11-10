using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public SettingsData settingsData;
    public VisualController visualController;


    [SerializeField] AudioSource menuSounds;
    public void LoadSceneByIndex(int index)
    {
        settingsData.LoadSettings(); // Load the player settings on scene change
        visualController.ApplySettings(settingsData);
        PlaySound.PlaySoundOnce("Menu_Click", menuSounds);
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }
}
