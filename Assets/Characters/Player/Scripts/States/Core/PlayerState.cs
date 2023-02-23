using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    // This is set by the Player FSM on Awake()
    protected PlayerController player_ref;

    public PlayerState(string name, FiniteStateMachine fsm) : base(name, fsm) { }

    public void SetPlayerReference(PlayerController p)
    {
        player_ref = p;
    }

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
