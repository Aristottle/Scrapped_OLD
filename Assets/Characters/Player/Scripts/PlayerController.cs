using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
        This class essentially contains all of the generic logic and/or helper methods that
        can be used by other scripts (namely the FSM) to drive the player logic. It also acts
        as a sort of repository for all of the player's parameters.
    */

    [Header("Control")]
    public float air_control = 0.1f;
    public float jump_force = 500f;
    public float jump_cooldown = .2f;

    [Header("Speeds")]
    private float movement_speed;
    [HideInInspector] public float desired_speed;
    public float walk_speed = 5f;
    public float sprint_speed = 8f;
    public float crouch_speed = 3f;
    public float wallrun_speed = 10f;
    public float acceleration = 10f;

    [Header("Crouching")]
    public float standing_height = 2f;
    public float crouch_height = 1f;

    [Header("Drag")]
    public float drag;
    public float ground_drag {get {return 8f;}}
    public float air_drag {get {return 0f;}}

    [Header("Ground Check")]
    [SerializeField] LayerMask ground_mask;
    [SerializeField] float ground_check_radius = 0.4f;
    private Vector3 ground_check;

    [Header("Slope Handling")]
    [SerializeField] float max_slope_angle = 45f;
    private RaycastHit slope_hit;

    [Header("References")]
    public CapsuleCollider capsule = null;
    public Rigidbody rb;
    public HeadBobController bob_controller;

    [HideInInspector] public bool is_grounded = true;
    bool do_ground_check = true;

    Vector3 movement_direction;


    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        bob_controller = GetComponent<HeadBobController>();

        standing_height = capsule.height;
        drag = ground_drag;
    }

    private void Update() 
    {
        if (do_ground_check)
        {
            // Keep track of whether we're grounded
            ground_check = new Vector3(capsule.transform.position.x, capsule.transform.position.y - capsule.height / 2, capsule.transform.position.z);
            is_grounded = Physics.CheckSphere(ground_check, ground_check_radius, ground_mask);
        }

        rb.drag = drag;
    }

    public Vector2 GetMovementInput()
    {
        float h_movement = Input.GetAxisRaw("Horizontal");
        float v_movement = Input.GetAxisRaw("Vertical");
        return new Vector2(h_movement, v_movement).normalized;
    }

    public void MovePlayer(float speed)
    {
        desired_speed = speed;
        // Lerp the movement speed to the desired_speed
        movement_speed = Mathf.Lerp(movement_speed, desired_speed * 10f, acceleration * Time.fixedDeltaTime);

        // Player's control is determined by whether or not they're in the air
        float control_multiplier = 1f;
        if (!is_grounded) control_multiplier = air_control;

        Vector2 movement_input = GetMovementInput();
        movement_direction = transform.forward * movement_input.y + transform.right * movement_input.x;

        Vector3 corrected_direction = movement_direction;

        if (OnSlope())
        {
            corrected_direction = GetSlopeMovementDirection();

            // Prevents weird bouncing when running up a slope
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        rb.AddForce(corrected_direction.normalized * movement_speed * rb.mass * control_multiplier, ForceMode.Force);

        // Keep the velocity clamped
        ClampVelocity();

        // Turn off gravity when we are on a slope
        rb.useGravity = !OnSlope();
    }

    private void ClampVelocity()
    {
        if (OnSlope())
        {
            if (rb.velocity.magnitude > desired_speed) rb.velocity = rb.velocity.normalized * desired_speed;
            return;
        }

        Vector3 flat_velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flat_velocity.magnitude > desired_speed)
        {
            Vector3 clamped_velocity = flat_velocity.normalized * desired_speed;
            rb.velocity = new Vector3(clamped_velocity.x, rb.velocity.y, clamped_velocity.z);
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slope_hit, capsule.height / 2f + ground_check_radius))
        {
            float angle = Vector3.Angle(Vector3.up, slope_hit.normal);
            return angle < max_slope_angle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(movement_direction, slope_hit.normal).normalized;
    }

    public void Crouch()
    {
        capsule.height = crouch_height;
        rb.AddForce(-transform.up * 500f, ForceMode.Impulse);
        PauseGroundCheck(0.3f);
    }

    public void Stand()
    {
        capsule.height = standing_height;
    }

    private void PauseGroundCheck(float duration)
    {
        do_ground_check = false;
        Invoke(nameof(ResumeGroundCheck), duration);
    }

    private void ResumeGroundCheck()
    {
        do_ground_check = true;
    }
}
