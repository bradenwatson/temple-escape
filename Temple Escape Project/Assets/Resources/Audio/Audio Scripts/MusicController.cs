using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for toggling on/off music with a bindable key, default 'X'
/// </summary>
public class MusicController : MonoBehaviour
{
    [SerializeField]
    AudioSource musicSource;
    [SerializeField] 
    bool musicPlaying = true;
    [SerializeField]
    KeyCode toggleMusicKey = KeyCode.X;
    void Start()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
    }
    void Update()
    {
        if(Input.GetKeyUp(toggleMusicKey))
        {
            if (musicPlaying)
            {
                PlaySound.StopSound(musicSource);
                musicPlaying = false;
            } else
            {
                PlaySound.PlaySoundOnRepeat("The_Secrets_of_Thoth", musicSource);
                musicPlaying = true;
            }
        }
    }    
}
