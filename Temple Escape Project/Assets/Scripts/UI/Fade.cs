//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image fadeScreen;
    public bool finishedFading = true;
    public float valueToBeDividedByDeltaTimeForSpeedChanging = 2f;

    bool isFading;
    float alphaValue;

    private void Start()
    {
        alphaValue = fadeScreen.color.a;
        finishedFading = false;
        isFading = false;
    }

    void Update()
    {
        if (!finishedFading)
        {
            if (isFading)
            {
                FadeScreen();
            }
            else
            {
                UnFadeScreen();
            }
        }
    }

    void FadeScreen()
    {
        if (alphaValue < 1)
        {
            alphaValue += Time.deltaTime * valueToBeDividedByDeltaTimeForSpeedChanging;
        }
        else
        {
            alphaValue = 1;
            finishedFading = true;
        }
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alphaValue);
    }

    void UnFadeScreen()
    {
        if (alphaValue > 0)
        {
            alphaValue -= Time.deltaTime * valueToBeDividedByDeltaTimeForSpeedChanging;
        }
        else
        {
            alphaValue = 0;
            finishedFading = true;
        }
        
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alphaValue);
    }

    public void SetFading(bool fading=true)
    {
        isFading = fading;
        finishedFading = false;
    }
}
