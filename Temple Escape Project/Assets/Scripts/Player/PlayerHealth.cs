using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // placeholder script - replace or modify where needed (enemy needs take damage function)
    public int maxHealth = 3;
    int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
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

    }

    private void MoveCollectablesBack()
    {

    }

    private void GameOverState()
    {

    }
}
