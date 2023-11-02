using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Audio;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu()]
public class SettingsManager : ScriptableObject
{
    [Header("File System")]
    public string settingsFileName = "/Settings.dat";

    [Header("Brightness")]
    public Image brightnessLayer;
    public float brightness = 1;

    [Header("Volume")]
    public AudioMixer audioMixer;
    public float maxAudio = 0f;
    public float minAudio = -80f;
    public float audioVolume = 1f;

    // Update brightness with new value.
    public void UpdateBrightness(float value)
    {
        brightness = value;
        float bufferValue = 1 - brightness;
        
        brightnessLayer.color = new Color(brightnessLayer.color.r, brightnessLayer.color.g, brightnessLayer.color.b, bufferValue);
    }

    // Update volume with new value.
    public void UpdateVolume(float value)
    {
        audioVolume = value;
        float range = maxAudio - minAudio;
        float volumeLevel = range * audioVolume;
        volumeLevel = minAudio + volumeLevel;

        audioMixer.SetFloat("MasterVolume", volumeLevel);
    }

    // Save the stored settings.
    public void SaveSettings()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + settingsFileName);

        SerialisableSettings settingsToSave = new SerialisableSettings(brightness, audioVolume);
        binaryFormatter.Serialize(file, settingsToSave);

        file.Close();
    }

    // Load settings from serialised file.
    public SerialisableSettings LoadSettings()
    {
        SerialisableSettings loadedSettings = new SerialisableSettings();

        if (File.Exists(Application.persistentDataPath + settingsFileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + settingsFileName, FileMode.Open);

            loadedSettings = binaryFormatter.Deserialize(file) as SerialisableSettings;

            file.Close();
        }

        return loadedSettings;
    }
}
