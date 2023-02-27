using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : FiniteStateMachine
{
    [HideInInspector] public PlayerIdle idle_state;
    [HideInInspector] public PlayerMovement ground_movement_state;
    [HideInInspector] public PlayerAir air_state;
    [HideInInspector] public PlayerSprint sprint_state;
    [HideInInspector] public PlayerCrouch crouch_state;
    [HideInInspector] public PlayerSlide slide_state;
    [HideInInspector] public PlayerWallrun wallrun_state;
    [HideInInspector] public PlayerGrapplePull grapple_pull_state;
    [HideInInspector] public PlayerGrappleSwing grapple_swing_state;

    private PlayerController player;

    private void Awake() 
    {
        // Get player controller
        player = GetComponent<PlayerController>();

        // Create states and add them to dictionary
        idle_state = new PlayerIdle(this);
        states.Add(idle_state.state_name, idle_state);
        ground_movement_state = new PlayerMovement(this);
        states.Add(ground_movement_state.state_name, ground_movement_state);
        air_state = new PlayerAir(this);
        states.Add(air_state.state_name, air_state);
        sprint_state = new PlayerSprint(this);
        states.Add(sprint_state.state_name, sprint_state);
        crouch_state = new PlayerCrouch(this);
        states.Add(crouch_state.state_name, crouch_state);
        slide_state = new PlayerSlide(this);
        states.Add(slide_state.state_name, slide_state);
        wallrun_state = new PlayerWallrun(this);
        states.Add(wallrun_state.state_name, wallrun_state);
        grapple_pull_state = new PlayerGrapplePull(this);
        states.Add(grapple_pull_state.state_name, grapple_pull_state);
        grapple_swing_state = new PlayerGrappleSwing(this);
        states.Add(grapple_swing_state.state_name, grapple_swing_state);

        // Set the references on all of the states
        foreach (PlayerState s in states.Values)
        {
            s.SetPlayerReference(player);
        }
    }

    protected override State GetInitialState()
    {
        return idle_state;
    }
}
