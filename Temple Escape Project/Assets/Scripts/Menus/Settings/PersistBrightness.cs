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

        // Don't destroy this game object on load.
        DontDestroyOnLoad(gameObject);

        // If brightnesssLayer is not saved, set it as the image component of the first child.
        if (settings.brightnessLayer == null)
        {
            settings.brightnessLayer = transform.GetChild(0).GetComponent<Image>();
        }
    }
}
