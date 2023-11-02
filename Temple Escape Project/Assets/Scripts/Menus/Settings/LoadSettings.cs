using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSettings : MonoBehaviour
{
    [Header("Data Script")]
    public SettingsManager settingsManager;

    [Header("Sliders")]
    public Slider brightnessSlider;
    public Slider volumeSlider;

    void Start()
    {
        // Load settings.
        SerialisableSettings loadedSettings = settingsManager.LoadSettings();
        
        // If there are no loaded settings, set as max values.
        if (loadedSettings == null)
        {
            brightnessSlider.value = brightnessSlider.maxValue;
            volumeSlider.value = volumeSlider.maxValue;
        }
        // If there are loaded settings, set as saved settings.
        else
        {
            brightnessSlider.value = loadedSettings.Brightness;
            volumeSlider.value = loadedSettings.Volume;
        }
    }
}
