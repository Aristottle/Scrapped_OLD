using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RegisterPlayerCharacter : MonoBehaviour
{
    private void Start()
    {
        // Register the player character with the PlayerManager
        PlayerManager.RegisterPlayerCharacter(gameObject);
    }
}

