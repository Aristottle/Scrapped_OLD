using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    [SerializeField] Transform respawn_point;

    private Transform player_transform;

    private void OnTriggerEnter(Collider other) {
        if (IsPlayer(other, out player_transform))
        {
            Rigidbody rb = player_transform.gameObject.GetComponentInChildren<Rigidbody>();
            if (rb != null) rb.position = respawn_point.position;
        }
    }

    private bool IsPlayer(Collider c, out Transform transform) 
    {
        transform = c.gameObject.transform.parent;
        return transform.gameObject.name == "Player";
    }
}
