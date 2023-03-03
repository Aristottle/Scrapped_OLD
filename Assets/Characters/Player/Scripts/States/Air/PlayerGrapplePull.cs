using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapplePull : PlayerState
{
    public PlayerGrapplePull(FiniteStateMachine fsm) : base("Grapple Pull", fsm) { }

    Vector3 hook_location = Vector3.zero;

    public void PullToPosition(Vector3 target_position)
    {
        // player_ref.clamp_velocity = false;

        player_ref.rb.drag = player_ref.air_drag;

        player_ref.desired_speed = player_ref.grapple_pull_speed;

        player_ref.rb.useGravity = false;

        // Apply force on the player in the direction of the target point
        Vector3 direction = (target_position - player_ref.transform.position).normalized;
        player_ref.rb.AddForce(direction * player_ref.grapple_pull_force, ForceMode.Force);
        // player_ref.rb.velocity = direction * player_ref.grapple_pull_speed;
    }

    public override void Enter(Dictionary<string, string> msg = null) 
    {
        base.Enter(msg);

        string hook_location_string = null;
        if (CheckEntryMessage(msg, "location", out hook_location_string))
        {
            hook_location = JsonUtility.FromJson<Vector3>(hook_location_string);
        }
        else
        {
            Debug.Log("ERR: no hook location fed to PlayerGrapplePull");
            state_machine.TransitionTo("Idle");
            return;
        }

        // Some impulse at the start adds a lot to the feel
        Vector3 direction = (hook_location - player_ref.transform.position).normalized;
        player_ref.rb.AddForce(direction * (player_ref.grapple_pull_force / 2), ForceMode.Impulse);

    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        if (!Input.GetButton("Ability"))
            state_machine.TransitionTo("Idle");
    }

    public override void UpdatePhysics() 
    {
        base.UpdatePhysics();
    
        // Pull the player to the location
        PullToPosition(hook_location);
    }

    public override void Exit() 
    {
        base.Exit();

        player_ref.ResetGrappling();
    }
}
