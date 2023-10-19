using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameDisplay : MonoBehaviour
{
    [Header("Game Over Display")]
    public GameObject gameOverDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //SceneManager.LoadScene(_nextSceneIndex);
            Debug.Log("Completed Game!!!");
            gameOverDisplay.SetActive(true);

            // wait for 3 seconds
            WaitForTime();

            gameOverDisplay.SetActive(false);
        }
    }

    private IEnumerator WaitForTime()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3f);
    }
}