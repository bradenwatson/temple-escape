using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example usage of PlaySoundOnRepeat with a fixed end time, starting and stopping on keypress
/// </summary>
public class PlayerMoveSound : MonoBehaviour
{
    [SerializeField]
    AudioSource source;
    [SerializeField]
    KeyCode triggerFootstepKey = KeyCode.K;
    [SerializeField]
    KeyCode stopSound = KeyCode.L;
    void Start()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(triggerFootstepKey))
        {
            StartCoroutine(PlaySound.PlaySoundOnRepeat("Player_Footstep", source, 5));
        }
        if (Input.GetKeyUp(stopSound))
        {
            PlaySound.StopSound(source);
        }
    }
}
