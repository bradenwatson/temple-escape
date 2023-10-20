using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableSettings
{
    float brightness = 0f;
    public float Brightness
    {
        get { return brightness; }
    }

    float volume = 0f;

    public float Volume
    {
        get { return volume; }
    }
    public SerializableSettings()
    {

    }

    public SerializableSettings(float _brightness, float _volume)
    {
        brightness = _brightness;
        volume = _volume;
    }
}
