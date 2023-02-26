using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallrun : PlayerState
{
    public PlayerWallrun(FiniteStateMachine fsm) : base("Wallrun", fsm) { }

    float time_elapsed = 0f;

    float tilt_amount = 10f;
    float fov_offset = 5f;

    public override void Enter(Dictionary<string, string> msg = null) 
    {
        base.Enter(msg);

        player_ref.drag = player_ref.wallrun_drag;

        player_ref.desired_speed = player_ref.wallrun_speed;

        // Dampen fall speed
        if (player_ref.rb.velocity.y < -1f)
            player_ref.rb.velocity = new Vector3(player_ref.rb.velocity.x, 0f, player_ref.rb.velocity.z);

        // Tilt camera
        player_ref.camera_fx.TiltCamera(player_ref.wall_right ? tilt_amount : tilt_amount * -1);
        // Apply fov change
        player_ref.camera_fx.OffsetFOV(fov_offset);

    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        player_ref.CheckWall();

        if (Input.GetButtonDown("Jump")) WallJump();

        // Wallrun timer
        time_elapsed += Time.deltaTime;
        if (time_elapsed >= player_ref.max_wallrun_time) FallOff();
    }

    public override void UpdatePhysics() 
    {
        base.UpdatePhysics();

        if (player_ref.CanWallrun())
        {
            if (player_ref.wall_left)
            {
                player_ref.Wallrun();
            }
            else if (player_ref.wall_right)
            {
                player_ref.Wallrun();
            }
            else
            {
                state_machine.TransitionTo("Air");
            }
        }
        else state_machine.TransitionTo("Air");
    }

    public override void Exit() 
    {
        base.Exit();

        player_ref.rb.useGravity = true;

        time_elapsed = 0f;

        // Reset camera tilt and fov
        player_ref.camera_fx.TiltCamera(0f);
        player_ref.camera_fx.OffsetFOV(-fov_offset);
    }

    private void WallJump()
    {
        // Calculate the horizontal force and combine it with the vertical force for the final
        Vector3 h_direction = player_ref.transform.forward + (player_ref.wall_left ? player_ref.wall_hit_left.normal : player_ref.wall_hit_right.normal);
        Vector3 final_force = (player_ref.transform.up * player_ref.wall_jump_v_force) + (h_direction * player_ref.wall_jump_h_force);
        // Reset y-velocity
        player_ref.rb.velocity = new Vector3(player_ref.rb.velocity.x, 0f, player_ref.rb.velocity.z);
        // Apply the jumping force to the player
        player_ref.rb.AddForce(final_force, ForceMode.Impulse);
        // Transition to air state
        state_machine.TransitionTo("Air");
    }

    private void FallOff()
    {
        Vector3 direction = Vector3.zero;
        if (player_ref.wall_left)
        {
            direction = player_ref.wall_hit_left.normal;
        }
        else if (player_ref.wall_right)
        {
            direction = player_ref.wall_hit_right.normal;
        }
        // Push away from wall
        player_ref.rb.AddForce(direction.normalized * 100, ForceMode.Impulse);
        // Push down
        player_ref.rb.AddForce(-player_ref.transform.up * 100, ForceMode.Impulse);

        state_machine.TransitionTo("Air");
    }
}
