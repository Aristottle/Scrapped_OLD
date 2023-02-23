using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : PlayerGrounded
{
    public PlayerSlide(FiniteStateMachine fsm) : base("Slide", fsm) { }

    public override void Enter(Dictionary<string, string> msg = null)
    {
        base.Enter();
        player_ref.drag = player_ref.ground_drag;
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        Vector2 movement_input = player_ref.GetMovementInput();

        // Ground Movement
        if (Mathf.Abs(movement_input.magnitude) < Mathf.Epsilon)
        {
            state_machine.TransitionTo("Idle");
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
