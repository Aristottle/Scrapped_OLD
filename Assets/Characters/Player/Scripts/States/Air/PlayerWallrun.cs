using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallrun : PlayerState
{
    public PlayerWallrun(FiniteStateMachine fsm) : base("Wallrun", fsm) { }

    float time_elapsed = 0f;

    public override void Enter(Dictionary<string, string> msg = null) 
    {
        base.Enter(msg);

        player_ref.drag = player_ref.wallrun_drag;

        player_ref.desired_speed = player_ref.wallrun_speed;

        // Dampen fall speed
        if (player_ref.rb.velocity.y < -1f)
            player_ref.rb.velocity = new Vector3(player_ref.rb.velocity.x, 0f, player_ref.rb.velocity.z);
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
        else
        {
            state_machine.TransitionTo("Air");
        }
    }

    public override void Exit() 
    {
        base.Exit();

        player_ref.rb.useGravity = true;

        time_elapsed = 0f;
    }

    private void WallJump()
    {
        Vector3 jump_direction = player_ref.transform.up + player_ref.transform.forward;
        if (player_ref.wall_left)
        {
            jump_direction += player_ref.wall_hit_left.normal;
        }
        else if (player_ref.wall_right)
        {
            jump_direction += player_ref.wall_hit_right.normal;
        }
        // Reset y-velocity
        player_ref.rb.velocity = new Vector3(player_ref.rb.velocity.x, 0f, player_ref.rb.velocity.z);
        // Apply the jumping force to the player
        player_ref.rb.AddForce(jump_direction.normalized * player_ref.wall_jump_force, ForceMode.Impulse);
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
