using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Audio;

public class VisualController : MonoBehaviour
{
    public SettingsData settingsData;
    public Slider brightnessSlider;
    public Slider bloomSlider;

    public Volume volume;

    private ColorAdjustments colorAdjustments;
    private Bloom bloom;


    private void Start()
    {
        // Loads all the saved settings on start. 

        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments); // Get the Color Adjustments settings from the volume
        volume.profile.TryGet<Bloom>(out bloom); // Get the Bloom settings from the volume

        SerializableSettings loadedSettings = settingsData.LoadSettings();
        AdjustBrightness(loadedSettings.Brightness);
        AdjustBloom(loadedSettings.BloomIntensity);
        brightnessSlider.value = loadedSettings.Brightness;
        bloomSlider.value = loadedSettings.BloomIntensity;
    }

    public void AdjustBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = value;
            settingsData.brightness = value; // update the brightness in SettingsData
            settingsData.SaveSettings(); // save immediately after adjusting
        }
    }
    public void AdjustBloom(float value)
    {
        if (bloom != null)
        {
            bloom.intensity.value = value;
            settingsData.bloomIntensity = value; // update the bloomIntensity in SettingsData
            settingsData.SaveSettings(); // save immediately after adjusting
        }
    }
    public void ApplySettings(SettingsData settingsData)
    {
        AdjustBrightness(settingsData.brightness);
        AdjustBloom(settingsData.bloomIntensity);
        settingsData.SaveSettings();
    }
}