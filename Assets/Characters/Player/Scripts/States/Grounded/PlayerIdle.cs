using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerGrounded
{
    public PlayerIdle(FiniteStateMachine fsm) : base("Idle", fsm) { }

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
        if (Mathf.Abs(movement_input.magnitude) > Mathf.Epsilon)
        {
            state_machine.TransitionTo("Walk");
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
    }

    public override void Exit() 
    {
        base.Exit();
    }
}