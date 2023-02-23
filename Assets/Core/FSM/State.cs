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

    protected bool CheckEntryMessage(Dictionary<string, string> _msg, string key, out string value)
    {
        if (_msg != null) 
        {
            _msg.TryGetValue(key, out value);
            if (value != null) return true;
        }
        value = null;
        return false;
    }

    // Virtual functions - passthroughs for functionality driven by the state machine
    public virtual void Enter(Dictionary<string, string> msg = null) {}
    public virtual void UpdateLogic() {}
    public virtual void UpdatePhysics() {}
    public virtual void Exit() {}

    // Can copy these into a new State for overrides:
    
    // public override void Enter(Dictionary<string, string> msg = null) 
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
