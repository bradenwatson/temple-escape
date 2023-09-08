using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class GameOver : MonoBehaviour
{
    public FadeToBlack gameOver;

    // Use this for initialization
    void Start()
    {        
    StartCoroutine(WaitForFade());
    }

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(gameOver.waitTime);
        gameOver.isFade = true;

    }

// Update is called once per frame
    void Update()
        {
        if (gameOver.isFade && RenderSettings.fogDensity < 1 && gameOver.light[0].intensity > 0)
        {
            gameOver.ChangeFogDensityPerSecond(gameOver.fogAcceleration);
            gameOver.ChangeLightIntensityPerSecond();
        }
    }
}
