using System.Collections;
using UnityEngine;

/// <summary>
/// Class which contains the list of all audio clips, and methods which can be called from other scripts to trigger or stop sounds
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
        // play sound clip from given source
        if (!source.isPlaying)
        {
            source.PlayOneShot(FindSound(clipName));
        }
    }

    // play sound on repeat for a specified duration, i.e. footsteps
    public static IEnumerator PlaySoundOnRepeat(string clipName, AudioSource source, float durationInSeconds)
    {
        if (source == null) { yield return null; } // if no source exists, no sound will play
        // find sound clip by name         
        source.clip = FindSound(clipName);
        // start playing sound on loop
        if (!source.isPlaying)
        {
            source.Play();
        }
        source.loop = true;
        // wait for specified duration
        yield return new WaitForSeconds(durationInSeconds);
        // stop playing sound
        source.Stop();
    }

    // play sound on repeat until stopped, i.e. music
    public static void PlaySoundOnRepeat(string clipName, AudioSource source)
    {
        if (source == null) { return; } // if no source exists, no sound will play
        // find sound clip by name 
        source.clip = FindSound(clipName);
        // start playing sound on loop
        if (!source.isPlaying)
        {
            source.Play();
        }
        source.loop = true;    
    }

    // end a sound started by PlaySoundOnRepeat without a set duration, or end one with a set duration early
    public static void StopSound(AudioSource source)
    {
        if (source != null && source.isPlaying) // if no source exists or not playing sound, nothing will happen
        {
            source.Stop();
        }
    }

    public static AudioClip FindSound(string clipName)
    {
        // find sound clip by name 
        foreach (AudioClip c in allClips)
        {
            if (c.name == clipName)
            {
                return c;
            }
        }
        return null;
    }
}
