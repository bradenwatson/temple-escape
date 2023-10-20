using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistBrightness : MonoBehaviour
{
    public SettingsData settings;
    public static GameObject brightnessObj;

    private void Awake()
    {
        if (brightnessObj != null)
        {
            Destroy(gameObject);
        }
        else
        {
            brightnessObj = gameObject;
        }
        DontDestroyOnLoad(gameObject);
        if (settings.brightnessImage == null)
        {
            settings.brightnessImage = transform.GetChild(0).GetComponent<Image>();
        }
    }
}
