using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : PlayerGrounded
{
    public PlayerCrouch(FiniteStateMachine fsm) : base("Crouch", fsm) { }

    bool wants_to_stop = false;

    public override void Enter(Dictionary<string, string> msg = null)
    {
        base.Enter();

        player_ref.drag = player_ref.ground_drag;
        player_ref.Crouch();
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        Vector2 movement_input = player_ref.GetMovementInput();

        if (Input.GetButtonDown("Crouch"))
            wants_to_stop = !wants_to_stop;

        // Crouch end
        if (wants_to_stop && player_ref.CanStand())
            state_machine.TransitionTo("Idle");
    }

    public override void UpdatePhysics() 
    {
        base.UpdatePhysics();

        player_ref.MovePlayer(player_ref.crouch_speed);
    }

    public override void Exit() 
    {
        base.Exit();

        player_ref.Stand();

        wants_to_stop = false;
    }
}
