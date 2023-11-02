using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenu : MonoBehaviour
{
    [Header("Switch Menu")]
    public GameObject menuToClose;
    public GameObject menuToOpen;

    // Open the next menu first, then close last one.
    public void Switch()
    {
        menuToOpen.SetActive(true);
        menuToClose.SetActive(false);
    }
}
