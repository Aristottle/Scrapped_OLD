using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FiniteStateMachine
{
    [HideInInspector] public PlayerIdle idle_state;
    [HideInInspector] public PlayerMovement ground_movement_state;
    [HideInInspector] public PlayerJump jump_state;
    [HideInInspector] public PlayerFalling air_state;

    private void Awake() 
    {
        // Create states and add them to dictionary
        idle_state = new PlayerIdle(this);
        states.Add(idle_state.state_name, idle_state);
        ground_movement_state = new PlayerMovement(this);
        states.Add(ground_movement_state.state_name, ground_movement_state);
        jump_state = new PlayerJump(this);
        states.Add(jump_state.state_name, jump_state);
        air_state = new PlayerFalling(this);
        states.Add(air_state.state_name, air_state);

        // Set the references on all of the states
        foreach (PlayerState s in states.Values)
        {
            
        }
    }

    protected override State GetInitialState()
    {
        return idle_state;
    }
}
