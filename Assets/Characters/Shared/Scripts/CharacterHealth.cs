using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

public class CharacterHealth : MonoBehaviour
{
    public UltEvent onDeath;

    private float currentHealth;

    private bool initialized = false;

    // Character health is initialized by external scripts
    public void InitializeHealth(EnemyProfile enemyProfile)
    {
        if (initialized) return;

        currentHealth = enemyProfile.baseMaxHealth;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;

        Debug.Log($"Took {amount} damage.");

        if (currentHealth <= 0)
            onDeath?.Invoke();

    }
}
