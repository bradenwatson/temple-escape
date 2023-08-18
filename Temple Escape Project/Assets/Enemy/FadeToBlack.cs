using System.Collections;
using UnityEngine;


public class FadeToBlack : MonoBehaviour
{

    private readonly float finalFogDensity = 1f;
    private readonly float finalLightIntensity = 0f;
    private float fadeTime;
    private Light[] light;

    public FadeToBlack(float fadeTime, Light[] light) 
    {
        this.fadeTime = fadeTime;
        this.light = light;
    }   


    public void EnableFog(bool fog)
    {
        RenderSettings.fog = fog;
    }

    public void SetFogColor(Color fogColor)
    {
        RenderSettings.fogColor = fogColor;
    }

    public void SetFogMode(FogMode mode)
    {
        RenderSettings.fogMode = mode;
    }

    public void SetFogDensity(float density)
    {
        RenderSettings.fogDensity = density;
    }

    public void SetLightIntensity(float intensity)
    {
        print(light);
        foreach(Light lightSource in light)
        {
            lightSource.intensity = intensity;
        }
    }

    // 0.6 starting fog, + x increase every frame = 1
    // 0.6 + 0.4 = 1
    // 0.6 + x * 2 seconds = 1
    // x represents the incremented increase every frame
    // x = 0.4/2
    // = 0.2 per second, but unknown per frame since it depends on framerate; need to use Time.deltaTime probably

    //based on the above, these methods hold the acceleration per second; need to convert to frame per second still
    // when you forget bimdas goddamn embaressing but funny as hell

    public float SetFogAcceleration(float startingFogDensity)
    {
        return (finalFogDensity - startingFogDensity) / fadeTime;
    }

    public float SetLightAcceleration(float startingLightIntensity)
    {
        return (finalLightIntensity - startingLightIntensity) / fadeTime;
    }

    public void ChangeFogDensityPerSecond(float fogDensityAcceleration)
    {
        RenderSettings.fogDensity += fogDensityAcceleration * Time.deltaTime;
    }

    public void ChangeLightIntensityPerSecond(float lightIntensityAcceleration)
    {
        foreach (Light lightSource in light)
        {
            lightSource.intensity += lightIntensityAcceleration * Time.deltaTime;
        }
    }
}
