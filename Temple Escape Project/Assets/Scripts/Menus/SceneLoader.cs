using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    

    [SerializeField] AudioSource menuSounds;
    public void LoadSceneByIndex(int index)
    {
        PlaySound.PlaySoundOnce("Menu_Click", menuSounds);
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }
}
