using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Player/PlayerMovementData")]

public class PlayerMovementData : ScriptableObject
{
	
    [Header("Basic Movement")]
    public float walk_speed = 4f;
    
}
