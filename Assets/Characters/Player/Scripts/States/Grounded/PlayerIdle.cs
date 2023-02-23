using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerGrounded
{
    public PlayerIdle(FiniteStateMachine fsm) : base("Idle", fsm) { }

    public override void Enter() 
    {
        base.Enter();
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        Vector2 movement_input = GetMovementInput();

        // Ground Movement
        if (Mathf.Abs(movement_input.magnitude) > Mathf.Epsilon)
        {
            state_machine.TransitionTo("Ground Movement");
        }
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            state_machine.TransitionTo("Jump");
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