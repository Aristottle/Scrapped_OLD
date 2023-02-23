using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public string state_name;
    protected FiniteStateMachine state_machine;

    // Constructor
    public State(string name, FiniteStateMachine fsm)
    {
        this.state_name = name;
        this.state_machine = fsm;
    }

    // Virtual functions - passthroughs for functionality driven by the state machine
    public virtual void Enter() {}
    public virtual void UpdateLogic() {}
    public virtual void UpdatePhysics() {}
    public virtual void Exit() {}

    // Can copy these into a new State for overrides:
    
    // public override void Enter() 
    // {
    //     base.Enter();
    // }

    // public override void UpdateLogic() 
    // {
    //     base.UpdateLogic();
    // }

    // public override void UpdatePhysics() 
    // {
    //     base.UpdatePhysics();
    // }

    // public override void Exit() 
    // {
    //     base.Exit();
    // }
}
