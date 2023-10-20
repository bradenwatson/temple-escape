using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // placeholder script - replace or modify where needed (enemy needs take damage function)
    [Header("stats")]
    public int maxHealth = 3;
    int currentHealth;

    [Header("positions")]
    Vector3 startingPosition;
    List<GameObject> collectables = new List<GameObject>();

    Fade fadeScript;
    public bool isRespawning = false;
    public bool playerIsDead = false;

    [SerializeField] AudioSource gameOverSource;

    void Start()
    {
        startingPosition = transform.position;
        currentHealth = maxHealth;
        gameObject.transform.position = startingPosition;
        fadeScript = gameObject.GetComponent<Fade>();
    }

    private void Update()
    {
        if (isRespawning && fadeScript.finishedFading)
        {
            MovePlayerBackToSpawn();
            fadeScript.SetFading(false);
            isRespawning=false;
        }
    }

    public void TakeDamage(int damage = 1)
    {
        if (!isRespawning)
        {
            currentHealth -= damage;
            Debug.Log("Player Hit!, Health = " + currentHealth);
            ResetPlayer();
            if (currentHealth == 0)
            {
                GameOverState();
            }
        }
    }

    private void ResetPlayer()
    {
        MoveCollectablesBack();
        isRespawning = true;
        fadeScript.SetFading(true);
    }

    private void MovePlayerBackToSpawn()
    {
        gameObject.transform.position = startingPosition;
    }

    private void MoveCollectablesBack()
    {
        foreach (var collectable in collectables)
        {
            var script = collectable.GetComponent<TempCollectableScript>();
            if (script != null)
            {
                script.PutCollectableBack();
            }
        }
    }

    private void GameOverState()
    {
        Debug.Log("Player Died");
        PlaySound.PlaySoundOnce("Game_Over", gameOverSource);
        playerIsDead = true;
    }
}
