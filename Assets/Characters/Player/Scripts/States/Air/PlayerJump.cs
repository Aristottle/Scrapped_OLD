using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerState
{
    public PlayerJump(FiniteStateMachine fsm) : base("Jump", fsm) { }

    public override void Enter() 
    {
        base.Enter();
        // Apply the jumping force to the player

        // Immediately transition to air state
        state_machine.TransitionTo("Air");
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();
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
