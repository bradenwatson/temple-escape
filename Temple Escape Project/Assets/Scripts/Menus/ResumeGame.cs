using UnityEngine;

public class ResumeGame : MonoBehaviour
{
    public PauseControl leftControllerPauseControl;
    public PauseControl rightControllerPauseControl;
    [SerializeField] AudioSource menuSounds;

    public void Resume()
    {
        if (leftControllerPauseControl)
        {
            leftControllerPauseControl.Resume();
            PlaySound.PlaySoundOnce("Menu_Click", menuSounds);
        }

        if (rightControllerPauseControl)
        {
            rightControllerPauseControl.Resume();
            PlaySound.PlaySoundOnce("Menu_Click", menuSounds);
        }

        Time.timeScale = 1f;
    }
}
