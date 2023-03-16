using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTarget : MonoBehaviour, IDamageable
{
    private float health = 100f;

    void IDamageable.Damage(float amount)
    {
        health -= amount;

        Debug.Log($"Hit target for {amount}. Ouch!");
        
        OnDamaged();
    }
    
    void OnDamaged()
    {
        if (health <= 0)
            Destroy(gameObject);
    }
}
