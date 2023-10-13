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

    int movement;

    public int Movement
    {
        get { return movement; }
    }

<<<<<<< HEAD
    public SerializableSettings()
    {

    }
=======
>>>>>>> 4d4016b46a62a115229e2e0d3e8b94da4959829d
    public SerializableSettings(float _brightness, float _volume, int _movement)
    {
        brightness = _brightness;
        volume = _volume;
        movement = _movement;
    }
}
