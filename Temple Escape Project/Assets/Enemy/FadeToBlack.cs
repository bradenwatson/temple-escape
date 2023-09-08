using System.Collections;
using UnityEngine;
using UnityEditor;


[CreateAssetMenuAttribute()]
public class FadeToBlack : ScriptableObject
{

    [Header("Fog Settings")]
    public bool fogExists;
    public Color fogColor;
    public FogMode mode;
    public float fogDensity;

    public float startingFogDensity;

    private float finalFogDensity = 1f;

    [Header("Light Settings")]
    public Light[] light;

    private float finalLightIntensity = 0f;

    [Header("Fade Settings")]
    public float fadeTime;
    public float waitTime;

    public bool isFade;
    public float fogAcceleration;
    

    public void EnableFog()
    {
        RenderSettings.fog = fogExists;
    }

    public void SetFogColor()
    {
        RenderSettings.fogColor = fogColor;
    }

    public void SetFogMode()
    {
        RenderSettings.fogMode = mode;
    }

    public void SetFogDensity()
    {
        RenderSettings.fogDensity = fogDensity;
    }

    

    // 0.6 starting fog, + x increase every frame = 1
    // 0.6 + 0.4 = 1
    // 0.6 + x * 2 seconds = 1
    // x represents the incremented increase every frame
    // x = 0.4/2
    // = 0.2 per second, but unknown per frame since it depends on framerate; need to use Time.deltaTime probably

    //based on the above, these methods hold the acceleration per second; need to convert to frame per second still
    // when you forget bimdas goddamn embaressing but funny as hell

    public float SetFogAcceleration()
    {
        return (finalFogDensity - startingFogDensity) / fadeTime;
    }

    public float SetLightAcceleration(float startingLightIntensity)
    {
        return (finalLightIntensity - startingLightIntensity) / fadeTime;
    }


    public void ChangeLightIntensityPerSecond()
    {
        foreach (Light lightSource in light)
        {
            lightSource.intensity += SetLightAcceleration(lightSource.intensity) * Time.deltaTime;
        }
    }

    public void ChangeFogDensityPerSecond(float fogDensityAcceleration)
    {
        RenderSettings.fogDensity += fogDensityAcceleration * Time.deltaTime;
    }


}
