using UnityEngine;

public class ResumeGame : MonoBehaviour
{
    public PauseControl leftControllerPauseControl;
    public PauseControl rightControllerPauseControl;

    public void Resume()
    {
        if (leftControllerPauseControl)
        {
            leftControllerPauseControl.Resume();
        }

        if (rightControllerPauseControl)
        {
            rightControllerPauseControl.Resume();
        }
    }
}
