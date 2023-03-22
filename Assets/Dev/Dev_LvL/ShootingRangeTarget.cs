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
            OnDeath();
    }

    private void OnDeath()
    {
        ToggleHidden(true);
        // Play the burst VFX
        death_fx?.Play();
    }

    public void ToggleHidden(bool hidden = true)
    {
        destroyed = hidden;
        if (destroyed)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponentInChildren<SphereCollider>().enabled = false;
        }
        else
        {
            health = 1;
            GetComponentInChildren<MeshRenderer>().enabled = true;
            GetComponentInChildren<SphereCollider>().enabled = true;
        }
    }
}
