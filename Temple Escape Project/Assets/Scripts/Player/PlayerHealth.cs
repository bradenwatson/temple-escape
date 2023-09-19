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


    void Start()
    {
        startingPosition = transform.position;
        currentHealth = maxHealth;
        gameObject.transform.position = startingPosition;
    }

    public void TakeDamage(int damage = 1)
    {
        currentHealth -= damage;
        ResetPlayer();
        if (currentHealth == 0)
        {
            GameOverState();
        }
    }

    private void ResetPlayer()
    {
        MoveCollectablesBack();
        MovePlayerBackToSpawn();
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

    }
}
