using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrappleSwing : PlayerState
{
    public PlayerGrappleSwing(FiniteStateMachine fsm) : base("Grapple Swing", fsm) { }
    
    public override void Enter(Dictionary<string, string> msg = null) 
    {
        base.Enter(msg);
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
