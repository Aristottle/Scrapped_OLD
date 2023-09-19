using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlayerManager
{

    private static List<GameObject> playerCharacters;

    #region Public Methods

    public static void RegisterPlayerCharacter(GameObject character)
    {
        // If the list isn't initialize, do so
        if (playerCharacters == null)
        {
            playerCharacters = new List<GameObject>();
        }

        if (playerCharacters.Contains<GameObject>(character) || character == null)
        {
            return;
        }
        playerCharacters.Append<GameObject>(character);

        Debug.Log("Player registered");
    }

    public static List<GameObject> GetPlayerCharacters()
    {
        return playerCharacters;
    }

    #endregion
}
