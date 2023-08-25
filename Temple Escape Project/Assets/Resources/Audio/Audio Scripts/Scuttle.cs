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
    void Start()
    {
        if (source == null)
        {
            source = gameObject.GetComponent<AudioSource>();
        } 
    }
    private void OnTriggerEnter(Collider other)
    {
        PlaySound.PlaySoundOnce("Bugs", source);
    }
}
