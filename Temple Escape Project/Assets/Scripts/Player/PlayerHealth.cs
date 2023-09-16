using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // temp script replace if you want. just make sure there is a function called take damage (needed for enemy)
    public int maxHealth = 3;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage = 1)
    {
        // meant to take only one damage at a time and then resets back to scratch on same level with less health
        currentHealth -= damage;
        RemoveAllCollectables();
        ResetPosition();
        if (currentHealth == 0)
        {
            GameOverState();
        }
    }

    private void RemoveAllCollectables()
    {
        // when player takes damage removes all collected items and teleports them back
    }

    private void ResetPosition()
    {
        // when player takes damage needs to reset to chosen location (most likely safe room)
    }

    private void GameOverState()
    {

    }
}
