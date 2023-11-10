using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableSettings
{
    private float brightness = 1f; // Sets brightness to an initial value
    public float Brightness => brightness; 

    private float volume = 1f; // Sets volume to initial value
    public float Volume => volume;

    private float bloomIntensity = 1f; // Sets bloom to initial value
    public float BloomIntensity => bloomIntensity;

    public SerializableSettings() { }

    public SerializableSettings(float _brightness, float _volume, float _bloomIntensity)
    {
        brightness = _brightness;
        volume = _volume;
        bloomIntensity = _bloomIntensity;
    }
}
