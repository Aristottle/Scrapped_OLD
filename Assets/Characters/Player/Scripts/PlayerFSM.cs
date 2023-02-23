using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FiniteStateMachine
{
    [HideInInspector] public PlayerIdle idle_state;
    [HideInInspector] public PlayerGroundMovement ground_movement_state;

    // Init the states and add them to the dict
    private void Awake() 
    {
        idle_state = new PlayerIdle(this);
        states.Add(idle_state.state_name, idle_state);
        ground_movement_state = new PlayerGroundMovement(this);
        states.Add(ground_movement_state.state_name, ground_movement_state);
    }

    protected override State GetInitialState()
    {
        return idle_state;
    }
}
