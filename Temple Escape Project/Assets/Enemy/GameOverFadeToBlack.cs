using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverFadeToBlack : MonoBehaviour
{
    public Light[] pointLight;

/*    public Animation jumpScareAnim;*/

    [Header("Fog Settings")]
    public Color fogColor;
    public float startingFogDensity;

    [Header("Light Settings")]
    public float startingLightIntensity;

    [Header("Fade Settings")]
    public float fadeTime;
    public float waitTime;

    public FadeToBlack gameOverToMainMenu;

    private bool isFade;
    private float fogAcceleration;
    private float lightAcceleration;
    

    // Start is called before the first frame update
    void Start()
    {

        gameOverToMainMenu.EnableFog(true);
        gameOverToMainMenu.SetFogColor(fogColor);
        gameOverToMainMenu.SetFogMode(FogMode.ExponentialSquared);
        gameOverToMainMenu.SetFogDensity(startingFogDensity);
        gameOverToMainMenu.SetLightIntensity(startingLightIntensity);

        
        fogAcceleration = gameOverToMainMenu.SetFogAcceleration(startingFogDensity);
        lightAcceleration = gameOverToMainMenu.SetLightAcceleration(startingLightIntensity);

        print(fogAcceleration);
        print(lightAcceleration);

        StartCoroutine(WaitForFade());

    }

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(waitTime);
        isFade = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isFade && RenderSettings.fogDensity < 1 && pointLight[0].intensity > 0)
        {
            gameOverToMainMenu.ChangeFogDensityPerSecond(fogAcceleration);
            gameOverToMainMenu.ChangeLightIntensityPerSecond(lightAcceleration);
        }
    }
}
