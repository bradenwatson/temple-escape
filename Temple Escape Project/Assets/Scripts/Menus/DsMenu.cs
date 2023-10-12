using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMenu : MonoBehaviour
{
    public PauseControl pauseControl;
    public GameObject menuToClose;

    public void Disable()
    {
        pauseControl.Resume();
        menuToClose.SetActive(false);
    }
}
