using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGrappleGun : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] FiniteStateMachine player_fsm;
    [SerializeField] float rotation_speed = 10f;

    // Update is called once per frame
    void Update()
    {
        Look();
    }

    void Look()
    {
        Vector3 look_rot;

        if (player_fsm.GetCurrentState() == "Grapple Pull" || player_fsm.GetCurrentState() == "Grapple Swing")
            look_rot = Quaternion.LookRotation(player.grapple_point - transform.position, Vector3.up).eulerAngles;
        else look_rot = transform.parent.eulerAngles;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(look_rot), Time.deltaTime * rotation_speed);
    }
}
