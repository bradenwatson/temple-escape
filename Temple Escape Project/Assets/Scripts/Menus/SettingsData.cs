using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu()]
public class Settings : ScriptableObject
{
    
    [Header("Brightness")]
    public Image brightnessImage;
    public float brightness = 1;

    [Header("Volume")]
    public AudioMixer masterMixer;
    public float maxAudio = 0f;
    public float minAudio = -80f;
    public float masterVolume = 1;

    [Header("File")]
    static FileStream file;
    const string Name = "Settings.dat";

    public int indexMovement;

    string CreateFilePath()
    {
        return Application.persistentDataPath + "/" + Name;
    }
    
    public void UpdateBrightness(float value)
    {
        brightness = value;
        float bufferValue = 1 - value;
        brightnessImage.color = new Color(brightnessImage.color.r, brightnessImage.color.g, brightnessImage.color.b, bufferValue);
    }

    public void UpdateVolume(float value)
    {
        masterVolume = value;
        float range = maxAudio - minAudio;
        float volumeLevel = range * masterVolume;
        volumeLevel = minAudio + volumeLevel;
        masterMixer.SetFloat("MasterVolume", volumeLevel);
    }

    public void UpdateDropdownValue(Dropdown dropdown)
    {
        if (indexMovement != dropdown.value)
        {
            indexMovement = dropdown.value;
        }
    }

    BinaryFormatter InstantiateBinaryFormatter()
    {
        return new BinaryFormatter();
    }

    public void SaveSettings()
    {
        file = File.Create(CreateFilePath());
        SerializableSettings settingsToSave = new(brightness, masterVolume, indexMovement);
        InstantiateBinaryFormatter().Serialize(file, settingsToSave);
        CloseFile(file);
    }

    void CloseFile(FileStream file)
    {
        file.Close();
    }

    public SerializableSettings LoadSettings()
    {
        SerializableSettings loadedSettings = new SerializableSettings();
        if (File.Exists(CreateFilePath()))
        {
            FileStream file = File.Open(CreateFilePath(), FileMode.Open);
            loadedSettings = (SerializableSettings)InstantiateBinaryFormatter().Deserialize(file);
            CloseFile(file);
        }
        return loadedSettings;
    }
}
