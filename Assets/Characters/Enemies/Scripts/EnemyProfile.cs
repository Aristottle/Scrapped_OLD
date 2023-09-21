using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI", menuName = "AI/EnemyProfile")]
public class EnemyProfile : ScriptableObject
{
    [Header("Info")]
    public float baseMaxHealth = 100f;

    [Header("Combat")]
    [Tooltip("This is in meters!")] public float desiredCombatRange = 3;
}