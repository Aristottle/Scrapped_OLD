using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeTarget : MonoBehaviour, IDamageable
{
    void IDamageable.Damage(float amount)
    {
        Debug.Log($"Hit target for {amount}. Ouch!");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
