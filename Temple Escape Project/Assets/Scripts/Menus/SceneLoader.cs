using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] AudioSource menuSounds;
    public void LoadSceneByIndex(int index)
    {
        PlaySound.PlaySoundOnce("Menu_Click", menuSounds);
=======


    public void LoadSceneByIndex(int index)
    {
        // reset any variables from a previous gamestate gameover or game complete
        Time.timeScale = 1f;

        // load scenes
>>>>>>> Stashed changes
        SceneManager.LoadScene(index);
        
    }
}
