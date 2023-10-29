using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BrightnessController : MonoBehaviour
{
    public SettingsData settingsData;
    private Slider brightnessSlider;

    private PostProcessVolume volume; // To store the PostProcessVolume
    private ColorGrading colorGrading; // To store the ColorGrading effect

    private void Start()
    {
    }
}
