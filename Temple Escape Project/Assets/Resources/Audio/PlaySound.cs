using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which contains the list of all audio clips, and methods which can be called from other scripts to trigger sounds
/// </summary>
public class PlaySound : MonoBehaviour
{
    public static Object[] allClips;

    private void Start()
    {
        // load all clips from Audio folder to array
        allClips = Resources.LoadAll("Audio", typeof(AudioClip));
    }

    // play requested sound once, i.e. pick up or open door
    public static void PlaySoundOnce(string clipName, AudioSource source)
    {
        if (source == null) { return; } // if no source exists, no sound will play
        // find sound clip by name 
        foreach(AudioClip c in allClips)
        {
            if(c.name == clipName)
            {
                // play sound clip from given source
                source.PlayOneShot(c);
                break;
            }
        }
    }

    // play sound on repeat for a specified duration, i.e. music
    public static IEnumerator PlaySoundOnRepeat(string clipName, AudioSource source, float durationInSeconds)
    {
        if (source == null) { yield return null; } // if no source exists, no sound will play
        // find sound clip by name 
        foreach (AudioClip c in allClips)
        {
            if (c.name == clipName)
            {
                source.clip = c;
                break;
            }
        }
        // start playing sound on loop
        source.Play();
        source.loop = true;
        // wait for specified duration
        yield return new WaitForSeconds(durationInSeconds);
        // stop playing sound
        source.Stop();
    }

    // play sound on repeat until stopped, i.e. footsteps
    public static IEnumerator PlaySoundOnRepeat(string clipName, AudioSource source)
    {
        if (source == null) { yield return null; } // if no source exists, no sound will play
        // find sound clip by name 
        foreach (AudioClip c in allClips)
        {
            if (c.name == clipName)
            {
                source.clip = c;
                break;
            }
        }
        // start playing sound on loop
        source.Play();
        source.loop = true;
        yield return null;
        // end sound by stopping the coroutine created on starting sound i.e. StopCoroutine(x);
    }
}
