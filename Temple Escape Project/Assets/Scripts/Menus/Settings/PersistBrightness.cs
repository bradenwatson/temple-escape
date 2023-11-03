using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistBrightness : MonoBehaviour
{
    [Header("Data Script")]
    public SettingsManager settings;

    [Header("Brightness")]
    public static GameObject brightnessObject;

    void Awake()
    {
        // If brightnessObject exists, destroy it.
        if (brightnessObject != null)
        {
            Destroy(gameObject);
        }
        // If not, set it as this game object.
        else
        {
            brightnessObject = gameObject;
        }

        // If brightnesssLayer is not saved, set it as the image component of the first child.
        if (settings.brightnessLayer == null)
        {
            settings.brightnessLayer = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        }

        SerialisableSettings loadedSettings = settings.LoadSettings();

        if (loadedSettings != null)
        {
            Image previousBrightnessImage = settings.brightnessLayer;
            float bufferValue = 1 - loadedSettings.Brightness;

            settings.brightnessLayer.color = new Color(
                previousBrightnessImage.color.r,
                previousBrightnessImage.color.g,
                previousBrightnessImage.color.b,
                bufferValue);
        }
    }
}
