using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprint : PlayerGrounded
{
    public PlayerSprint(FiniteStateMachine fsm) : base("Sprint", fsm) { }

    public override void Enter(Dictionary<string, string> msg = null)
    {
        base.Enter();

        player_ref.drag = player_ref.ground_drag;
        
        player_ref.bob_controller.amp_multiplier = 1.5f;
        player_ref.bob_controller.freq_multiplier = 1.5f;
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        Vector2 movement_input = player_ref.GetMovementInput();

        // No movement_input
        if (Mathf.Abs(movement_input.magnitude) < Mathf.Epsilon)
        {
            state_machine.TransitionTo("Idle");
        }
        // Sprint released or not enough forward movement input
        if (movement_input.y < .5 || !Input.GetButton("Sprint"))
        {
            state_machine.TransitionTo("Walk");
        }
        // Slide
        if (Input.GetButtonDown("Crouch"))
        {
            // state_machine.TransitionTo("Slide");
        }
    }

    public override void UpdatePhysics() 
    {
        base.UpdatePhysics();

        player_ref.MovePlayer(player_ref.sprint_speed);
    }

    public override void Exit() 
    {
        base.Exit();

        player_ref.bob_controller.amp_multiplier = 1f;
        player_ref.bob_controller.freq_multiplier = 1f;
    }
}
