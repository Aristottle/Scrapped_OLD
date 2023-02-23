using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFalling : PlayerState
{
    public PlayerFalling(FiniteStateMachine fsm) : base("Air", fsm) { }

    public override void Enter() 
    {
        base.Enter();
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();
        // If the player touches the ground, transition to idle (they will immediately transfer to move from there if there's movement input)
        
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
