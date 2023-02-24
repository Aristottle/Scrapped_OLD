using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : PlayerGrounded
{
    public PlayerSlide(FiniteStateMachine fsm) : base("Slide", fsm) { }

    bool stay_crouched = false;

    public override void Enter(Dictionary<string, string> msg = null)
    {
        base.Enter();
        player_ref.drag = player_ref.slide_drag;
        player_ref.Crouch();
        // Apply the drag impulse to the player
        Vector3 impulse_direction = (player_ref.transform.forward - player_ref.transform.up).normalized;
        player_ref.rb.AddForce(impulse_direction * player_ref.slide_impulse, ForceMode.Impulse);
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        Vector2 movement_input = player_ref.GetMovementInput();

        // If we go below walking speed, stop the slide
        if (player_ref.rb.velocity.magnitude < player_ref.walk_speed)
        {
            if (player_ref.GetMovementInput().y >= .5f && Input.GetButton("Sprint"))
                state_machine.TransitionTo("Sprint");
            else
            {
                stay_crouched = true;
                state_machine.TransitionTo("Crouch");
            }
        }
    }

    public override void UpdatePhysics() 
    {
        base.UpdatePhysics();
    }

    public override void Exit() 
    {
        base.Exit();
        if (!stay_crouched) player_ref.Stand();
        stay_crouched = false;
    }
}
