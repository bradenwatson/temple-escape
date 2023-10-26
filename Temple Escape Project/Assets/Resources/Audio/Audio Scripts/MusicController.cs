using UnityEngine;

/// <summary>
/// Script for toggling on/off music with a bindable key, default 'M'
/// </summary>
public class MusicController : MonoBehaviour
{
    [SerializeField]
    static AudioSource musicSource;
    [SerializeField]
    static bool musicPlaying = true;
    [SerializeField]
    KeyCode toggleMusicKey = KeyCode.M;
    void Start()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
    }
    void Update()
    {
        if (Input.GetKeyUp(toggleMusicKey))
        {
            ToggleMusic();
        }
    }
    public static void ToggleMusic()
    {
        if (musicPlaying)
        {
            PlaySound.StopSound(musicSource);
            musicPlaying = false;
        }
        else
        {
            PlaySound.PlaySoundOnRepeat("The_Secrets_of_Thoth", musicSource);
            musicPlaying = true;
        }
    }
}