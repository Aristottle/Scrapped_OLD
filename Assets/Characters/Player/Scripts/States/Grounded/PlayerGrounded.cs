using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerState
{
    public PlayerGrounded(string name, FiniteStateMachine fsm) : base(name, fsm) { }

    public override void Enter(Dictionary<string, string> msg = null) 
    {
        base.Enter();
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        // If the player isn't grounded, transition to air
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
