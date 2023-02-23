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
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            Dictionary<string, string> _msg = new Dictionary<string, string>();
            _msg.Add("jump", "true");
            state_machine.TransitionTo("Air", _msg);
        }
        else if (!player_ref.is_grounded)
        {
            state_machine.TransitionTo("Air");
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
