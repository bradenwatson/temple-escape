using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMenu : MonoBehaviour
{
    public GameObject menuToClose;

    public void Disable()
    {
        menuToClose.SetActive(false);
    }
}
