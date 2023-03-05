using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapplePull : PlayerState
{
    public PlayerGrapplePull(FiniteStateMachine fsm) : base("Grapple Pull", fsm) { }

    Vector3 hook_location = Vector3.zero;
    float time_elapsed = 0;

    float fov_offset = 5f;

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

        // As the player gets closer to the destination, apply a counter force to their tangential movement so they
        // go right to the destination. Should start at a certain distance and the effect should scale inversely to
        // said distance. 
        Vector3 velocity = player_ref.rb.velocity;
        Vector3 tangential_velocity = Vector3.ProjectOnPlane(velocity, direction);
        float distance = (target_position - player_ref.transform.position).magnitude;
        if (distance <= player_ref.grapple_counterforce_start_distance)
            player_ref.rb.velocity -= tangential_velocity * (1 - distance / player_ref.grapple_counterforce_start_distance);

        // End the grapple if they reach their destination
        if (distance <= player_ref.grapple_end_tolerance)
            state_machine.TransitionTo("Air");
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

        // Effects
        // Apply fov change
        player_ref.camera_fx.OffsetFOV(fov_offset);
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        if (!Input.GetButton("Ability"))
            state_machine.TransitionTo("Air");

        if (time_elapsed >= player_ref.max_grapple_time)
            state_machine.TransitionTo("Air");
        
        time_elapsed += Time.deltaTime;
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

        time_elapsed = 0f;

        player_ref.camera_fx.OffsetFOV(-fov_offset);

        player_ref.Invoke(nameof(player_ref.ResetGrappling), player_ref.grapple_cooldown);
    }
}
