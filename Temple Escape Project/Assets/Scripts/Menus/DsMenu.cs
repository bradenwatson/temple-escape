using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMenu : MonoBehaviour
{
    public GameObject menuToClose;
    [SerializeField] AudioSource menuSounds;

    public void Disable()
    {
        menuToClose.SetActive(false);
        PlaySound.PlaySoundOnce("Menu_Click", menuSounds);
    }
}
