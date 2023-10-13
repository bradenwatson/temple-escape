using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLoad : MonoBehaviour
{
    public Settings masterSettings;
    public Slider brightnessSlider;
    public Slider volumeSlider;

    void Start()
    {
        SerializableSettings loadedSettings = masterSettings.LoadSettings();
        if (loadedSettings == null )
        {
            brightnessSlider.value = brightnessSlider.maxValue;
            volumeSlider.value = volumeSlider.maxValue;
        }
        else
        {
            brightnessSlider.value = loadedSettings.Brightness;
            volumeSlider.value = loadedSettings.Volume;
        }
    }
}
