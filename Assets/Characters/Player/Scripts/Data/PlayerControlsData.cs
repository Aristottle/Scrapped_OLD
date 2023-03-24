using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControlData", menuName = "Player/PlayerControlData")]

public class PlayerControlsData : ScriptableObject
{
	
    [Header("Look Input")]
    public float mouse_sensitivity = .5f;
    
}
