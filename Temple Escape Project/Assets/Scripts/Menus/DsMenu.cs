using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMenu : MonoBehaviour
{
    public PauseControl leftControllerPauseControl;
    public PauseControl rightControllerPauseControl;
    public GameObject menuToClose;

    public void Disable()
    {
        leftControllerPauseControl.Resume();
        rightControllerPauseControl.Resume();
        menuToClose.SetActive(false);
    }
}
