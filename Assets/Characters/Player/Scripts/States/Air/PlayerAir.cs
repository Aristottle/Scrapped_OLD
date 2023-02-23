using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAir : PlayerState
{
    public PlayerAir(FiniteStateMachine fsm) : base("Air", fsm) { }

    public override void Enter(Dictionary<string, string> msg) 
    {
        base.Enter();
        // Reset y-velocity
        player_ref.rb.velocity = new Vector3(player_ref.rb.velocity.x, 0f, player_ref.rb.velocity.z);
        // Apply the jumping force to the player
        player_ref.rb.AddForce(player_ref.transform.up * player_ref.jump_force, ForceMode.Impulse);
        // Immediately transition to air state
        state_machine.TransitionTo("Air");
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
