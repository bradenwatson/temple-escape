using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEditor;

[CreateAssetMenu()]
public class Settings : ScriptableObject
{
    public Image brightnessImage;
    public float brightness = 1;
    public AudioMixer masterMixer;
    public float maxAudio = 0f;
    public float minAudio = -80f;
    public float masterVolume = 1;

    public void UpdateBrightness(float value)
    {
        value -= 1;
        brightness = value;
        brightnessImage.color = new Color(brightnessImage.color.r, brightnessImage.color.g, brightnessImage.color.b, brightness);
    }
}
