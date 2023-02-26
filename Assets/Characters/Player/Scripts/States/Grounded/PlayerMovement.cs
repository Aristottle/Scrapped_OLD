using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerGrounded
{
    public PlayerMovement(FiniteStateMachine fsm) : base("Walk", fsm) { }

    public override void Enter(Dictionary<string, string> msg = null)
    {
        base.Enter();

        player_ref.drag = player_ref.ground_drag;

        player_ref.camera_fx.SetHeadBobMultipliers();
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        Vector2 movement_input = player_ref.GetMovementInput();

        // Idle
        if (Mathf.Abs(movement_input.magnitude) < Mathf.Epsilon)
        {
            state_machine.TransitionTo("Idle");
        }
        // Sprint
        if (movement_input.y >= .5 && Input.GetButton("Sprint"))
        {
            state_machine.TransitionTo("Sprint");
        }
        // Crouch
        if (Input.GetButtonDown("Crouch"))
        {
            state_machine.TransitionTo("Crouch");
        }
    }

    public override void UpdatePhysics() 
    {
        base.UpdatePhysics();

        player_ref.MovePlayer(player_ref.walk_speed);
    }

    public override void Exit() 
    {
        base.Exit();
    }
}
