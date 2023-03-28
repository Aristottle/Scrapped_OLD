using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialCheckpoint : MonoBehaviour
{
    public delegate void PlayerEnter(bool start, GameObject player);
    public static event PlayerEnter PlayerEntered;
    
    public bool start;

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
        // Just makes the trigger enter subscribable
        if (PlayerEntered != null)
            PlayerEntered(start, other.gameObject);
    }
}
