using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlide : PlayerGrounded
{
    public PlayerSlide(FiniteStateMachine fsm) : base("Slide", fsm) { }

    bool stay_crouched = false;

    float tilt_amount = 7.5f;
    float fov_offset = 5f;

    public override void Enter(Dictionary<string, string> msg = null)
    {
        base.Enter();
        player_ref.desired_speed = player_ref.terminal_velocity;
        player_ref.drag = player_ref.slide_drag;
        player_ref.Crouch();
        // Apply the slide impulse to the player
        Vector3 impulse_direction = (((player_ref.transform.forward * player_ref.slide_direction_influence) + (player_ref.rb.velocity.normalized * (1f - player_ref.slide_direction_influence))) - player_ref.transform.up).normalized;
        player_ref.rb.AddForce(impulse_direction * player_ref.slide_impulse, ForceMode.Impulse);


        player_ref.camera_fx.ToggleHeadBob(false);
        // Tilt camera
        player_ref.camera_fx.TiltCamera(tilt_amount);
        // Apply fov change
        player_ref.camera_fx.OffsetFOV(fov_offset);
    }

    public override void UpdateLogic() 
    {
        base.UpdateLogic();

        Vector2 movement_input = player_ref.GetMovementInput();

        // If we go below walking speed, stop the slide
        if (player_ref.rb.velocity.magnitude < player_ref.walk_speed)
        {
            if (player_ref.GetMovementInput().y >= .5f && Input.GetButton("Sprint") && player_ref.CanStand())
            {
                state_machine.TransitionTo("Sprint");
            }
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
        
        // Reset camera tilt and fov
        player_ref.camera_fx.TiltCamera(0f, 0.1f);
        player_ref.camera_fx.OffsetFOV(-fov_offset);

        if (!stay_crouched) player_ref.Stand();
        stay_crouched = false;

    }
}
