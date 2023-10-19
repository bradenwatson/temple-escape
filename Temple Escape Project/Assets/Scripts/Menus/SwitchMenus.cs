using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMenus : MonoBehaviour
{
    public GameObject menuToClose;
    public GameObject menuToOpen;

    public void Switch()
    {
        menuToOpen.SetActive(true);
        menuToClose.SetActive(false);
    }
}
