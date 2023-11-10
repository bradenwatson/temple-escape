using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering;
using Unity.XR.CoreUtils;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[CreateAssetMenu()]
public class SettingsData : ScriptableObject
{
    public float brightness = 1;
    public float bloomIntensity = 1;
    public AudioMixer masterMixer;
    public float maxAudio = 0f;
    public float minAudio = -80f;
    public float masterVolume = 1;

    public void UpdateVolume(float value)
    {
        masterVolume = value;
        float range = maxAudio - minAudio;
        float volumeLevel = range * masterVolume;
        volumeLevel = minAudio + volumeLevel;
        masterMixer.SetFloat("MasterVolume", volumeLevel);
    }

    public void SaveSettings()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Settings.dat");
        SerializableSettings settingsToSave = new SerializableSettings(brightness, masterVolume, bloomIntensity); // Added bloomIntensity
        bf.Serialize(file, settingsToSave);
        file.Close();
    }

    public SerializableSettings LoadSettings()
    {
        SerializableSettings loadsettings = new SerializableSettings();
        if (File.Exists(Application.persistentDataPath + "/Settings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Settings.dat", FileMode.Open);
            loadsettings = (SerializableSettings)bf.Deserialize(file);
            file.Close();
        }
        return loadsettings;
    }
}