using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerialisableSettings
{
    [Header("Brightness")]
    float brightness = 0f;
    public float Brightness
    {
        get { return brightness; }
    }

    [Header("Volume")]
    float volume = 0f;
    public float Volume
    {
        get { return volume; }
    }

    // Default constructor with no values required.
    public SerialisableSettings() { }

    // Constructor with two values: _brightness and _volume.
    public SerialisableSettings(float _brightness, float _volume)
    {
        brightness = _brightness;
        volume = _volume;
    }
}
