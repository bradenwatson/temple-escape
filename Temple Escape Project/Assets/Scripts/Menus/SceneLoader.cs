using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //public GameObject leftTeleportRay;
    //public GameObject rightTeleportRay;

    [SerializeField] AudioSource menuSounds;
    public void LoadSceneByIndex(int index)
    {
        PlaySound.PlaySoundOnce("Menu_Click", menuSounds);
        Time.timeScale = 1f;
        //leftTeleportRay.gameObject.SetActive(true);
        //rightTeleportRay.gameObject.SetActive(true);
        SceneManager.LoadScene(index);
    }
}
