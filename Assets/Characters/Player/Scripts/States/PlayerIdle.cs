using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(FiniteStateMachine fsm) : base("Idle", fsm) { }

    // OVERRIDES:

    public override void Enter() 
    {

    }

    public override void UpdateLogic() 
    {
        Vector2 movement_input = GetMovementInput();

        // Ground Movement
        if (Mathf.Abs(movement_input.magnitude) > Mathf.Epsilon)
        {
            state_machine.TransitionTo("Ground Movement");
        }
    }

    public override void UpdatePhysics() 
    {

    }

    public override void Exit() 
    {

    }
}