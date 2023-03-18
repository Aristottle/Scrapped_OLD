using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTarget : MonoBehaviour, IDamageable
{
    private float health = 1;
    [HideInInspector] public bool destroyed = false;

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
