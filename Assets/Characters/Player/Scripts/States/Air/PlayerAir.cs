using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAir : PlayerState
{
    public PlayerAir(FiniteStateMachine fsm) : base("Air", fsm) { }

    private bool can_air_jump = true;

    private void Jump()
    {
        // Disable the ground check for a duration
        // player_ref.PauseGroundCheck(.3f);
        // Reset y-velocity
        player_ref.rb.velocity = new Vector3(player_ref.rb.velocity.x, 0f, player_ref.rb.velocity.z);
        // Apply the jumping force to the player
        player_ref.rb.AddForce(player_ref.transform.up * player_ref.jump_force, ForceMode.Impulse);
    }

    public override void Enter(Dictionary<string, string> msg) 
    {
        base.Enter();
        // Jump if the msg indicates that we should
        player_ref.drag = player_ref.air_drag;
        string should_jump = string.Empty;
        if (CheckEntryMessage(msg, "jump", out should_jump)) Jump();
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        // Wallrun check
        player_ref.CheckWall();

        if (player_ref.is_grounded)
        {
            Vector3 flat_velocity = new Vector3(player_ref.rb.velocity.x, 0, player_ref.rb.velocity.z);
            // If we're going faster than walk_speed horizontally and the crouch button is held, slide
            if (flat_velocity.magnitude >= player_ref.walk_speed && Input.GetButton("Crouch"))
            {
                state_machine.TransitionTo("Slide");
                return;
            }
            state_machine.TransitionTo("Idle");
        }
        // Air jump
        if (Input.GetButtonDown("Jump") && can_air_jump)
        {
            can_air_jump = false;
            Jump();
        }
        // Wallrunning
        if (player_ref.CanWallrun())
        {
            if (player_ref.wall_left || player_ref.wall_right)
            {
                state_machine.TransitionTo("Wallrun");
            }
        }
    }

    public override void UpdatePhysics() 
    {
        base.UpdatePhysics();
        player_ref.MovePlayer(player_ref.desired_speed);
    }

    public override void Exit() 
    {
        base.Exit();
        can_air_jump = true;
    }
}
