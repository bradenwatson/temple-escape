using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthTest : MonoBehaviour
{
    public float maxHealth = 5f;
    public float currentHealth = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageTaken = 1)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Destroy(gameObject);
    }
}
