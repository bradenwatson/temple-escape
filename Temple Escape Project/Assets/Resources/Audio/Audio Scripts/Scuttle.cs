using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Demo of triggering a sound when moving into range of something, like a puzzle piece or enemy
/// </summary>
public class Scuttle : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    private void OnTriggerEnter(Collider other)
    {
        if (source == null)
        {
            AudioSource.PlayClipAtPoint(PlaySound.FindSound("Bugs"), transform.position);
        }
        PlaySound.PlaySoundOnce("Bugs", source);
    }
}
