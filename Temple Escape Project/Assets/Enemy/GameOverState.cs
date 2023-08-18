using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverState : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("Test the gameover state")]
    public bool gameOver = false;

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            gameOver = true;
        }
    }



    private void Update()
    {
        if (gameOver)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

}
