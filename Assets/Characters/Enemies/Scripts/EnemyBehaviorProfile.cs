using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI", menuName = "AI/EnemyBehaviorProfiles")]
public class EnemyBehaviorProfile : ScriptableObject
{
    [Header("Combat")]
    [Tooltip("This is in meters!")] public float desiredCombatRange = 3;
}
