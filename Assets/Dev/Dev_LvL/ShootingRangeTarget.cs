using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShootingRangeTarget : MonoBehaviour, IDamageable
{
    private float health = 1;
    [HideInInspector] public bool destroyed = false;

    private VisualEffect death_fx;

    private void Start() 
    {
        death_fx = GetComponentInChildren<VisualEffect>();
    }

    void IDamageable.Damage(float amount)
    {
        health -= amount;

        Debug.Log($"Hit target for {amount}. Ouch!");
        
        OnDamaged();
    }
    
    void OnDamaged()
    {
        if (health <= 0)
            ToggleHidden(true);

    }

    private void OnDeath()
    {
        // Play the burst VFX
        death_fx?.Play();
    }

    public void ToggleHidden(bool hidden = true)
    {
        destroyed = hidden;
        if (destroyed)
        {
            gameObject.SetActive(false);
        }
        else
        {
            health = 1;
            gameObject.SetActive(true);
        }
    }
}
