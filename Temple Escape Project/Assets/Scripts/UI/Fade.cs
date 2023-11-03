//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public bool startFadingOnLaunch = true;
    public Image fadeImage;

    public float maxAlphaValue = 1.0f;
    public float minAlphaValue = 0f;

    public float fadeTime = 1.5f;
    public AnimationCurve fadeCurve;

    public bool finishedFading = false;

    Color fadeColour;

    void Start()
    {
        print("fade");
        fadeColour = fadeImage.color;
        SetFading(startFadingOnLaunch);
    }

    IEnumerator FadeIn()
    {
        finishedFading = false;
        float timer = fadeTime;
        float index;
        while (fadeColour.a > minAlphaValue)
        {
            timer -= Time.deltaTime;
            index = timer / fadeTime;

            print($"{index} : {timer}");
            fadeColour.a = fadeCurve.Evaluate(index);

            fadeImage.color = fadeColour;
            yield return null;
        }
        finishedFading = true;
    }

    IEnumerator FadeOut()
    {
        finishedFading = false;
        float timer = 0;
        float index;
        while (fadeColour.a < maxAlphaValue)
        {
            timer += Time.deltaTime;
            index = timer / fadeTime;

            print($"{index} : {timer}");
            fadeColour.a = fadeCurve.Evaluate(index);

            fadeImage.color = fadeColour;
            yield return null;
        }
        finishedFading = true;
    }

    public void SetFading(bool fading=true)
    {
        StopAllCoroutines();
        if (fading)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }
}
