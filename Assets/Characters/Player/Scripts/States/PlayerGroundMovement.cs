using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundMovement : PlayerState
{
    public PlayerGroundMovement(FiniteStateMachine fsm) : base("Ground Movement", fsm) { }

    public override void Enter() 
    {

    }

    public override void UpdateLogic() 
    {
        Vector2 movement_input = GetMovementInput();

        // Ground Movement
        if (Mathf.Abs(movement_input.magnitude) < Mathf.Epsilon)
        {
            state_machine.TransitionTo("Idle");
        }
    }

    public override void UpdatePhysics() 
    {

    }

    public override void Exit() 
    {

    }
}
