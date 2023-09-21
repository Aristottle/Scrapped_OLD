using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

// This class just allows us to attach this component to anything that needs to be damageable
// and drive logic through events.

public class DamageableObject : MonoBehaviour, IDamageable
{
    public UltEvent<float> onDamageTaken;

    // We can use the multiplier to define weak or armored areas on characters.
    [SerializeField] float damageMultiplier = 1;

    // IDamageable
    public void Damage(float amount)
    {
        onDamageTaken?.Invoke(amount * damageMultiplier);
    }
}
