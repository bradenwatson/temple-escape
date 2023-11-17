using UnityEngine;
using UnityEngine.Audio;

public class PersistVolume : MonoBehaviour
{
    [Header("Data Script")]
    public SettingsManager settings;

    [Header("Audio")]
    public AudioMixer audioController;

    [Header("Volume")]
    private string masterVolume = "MasterVolume";
    private float minVolume = -80f;
    private float maxVolume = 0f;

    /// <summary>
    /// Initialises audio settings by loading saved settings
    /// and applies the volume setting to the audio controller.
    /// </summary>
    private void Awake()
    {
        SerialisableSettings loadedSettings = settings.LoadSettings();

        float audioVolume = loadedSettings.Volume;
        float range = maxVolume - minVolume;
        float volumeLevel = range * audioVolume;
        volumeLevel = minVolume + volumeLevel;

        audioController.SetFloat(masterVolume, volumeLevel);
    }
}
