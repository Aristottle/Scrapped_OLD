using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Control")]
    [SerializeField] float air_control = 0.1f;
    [SerializeField] float jump_force = 500f;
    [SerializeField] float jump_cooldown = .2f;
    private bool can_jump = true;

    [Header("Speeds")]
    private float movement_speed;
    private float desired_speed;
    public float walk_speed = 5f;
    public float sprint_speed = 8f;
    public float crouch_speed = 3f;
    public float wallrun_speed = 10f;
    public float acceleration = 10f;

    [Header("Crouching")]
    [SerializeField] float crouch_height = 1f;

    [Header("Drag")]
    [SerializeField] float ground_drag = 8f;
    [SerializeField] float air_drag = 0f;

    [Header("Ground Check")]
    [SerializeField] LayerMask ground_mask;
    [SerializeField] float ground_check_radius = 0.4f;
    private Vector3 ground_check;

    [Header("Slope Handling")]
    [SerializeField] float max_slope_angle = 45f;
    private RaycastHit slope_hit;

    [Header("References")]
    public CapsuleCollider capsule = null;
    [SerializeField] Transform eyes = null;

    float h_movement;
    float v_movement;
    Vector3 movement_direction;
    [HideInInspector] public bool is_grounded;

    Rigidbody rb;
    HeadBobController bob_controller;

    float player_height = 2f;

    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air,
    }

    [HideInInspector] public MovementState movement_state = MovementState.walking;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        bob_controller = GetComponent<HeadBobController>();

        player_height = capsule.height;

        // Start at walking speed
        desired_speed = walk_speed;
    }

    private void Update()
    {
        ground_check = new Vector3(capsule.transform.position.x, capsule.transform.position.y - capsule.height / 2, capsule.transform.position.z);
        is_grounded = Physics.CheckSphere(ground_check, ground_check_radius, ground_mask);

        HandleInput();
        HandleStates();

        // Handle drag (will move this to states)
        if (is_grounded) rb.drag = ground_drag;
        else rb.drag = air_drag;
    }

    void HandleInput()
    {
        h_movement = Input.GetAxisRaw("Horizontal");
        v_movement = Input.GetAxisRaw("Vertical");

        movement_direction = transform.forward * v_movement + transform.right * h_movement;

        // Jumping
        if (Input.GetButtonDown("Jump") && can_jump && is_grounded) 
        {
            can_jump = false;
            Jump();
            Invoke(nameof(ResetJump), jump_cooldown);
        }

        // Crouching
        if (Input.GetButtonDown("Crouch"))
        {
            if (is_grounded) Crouch();
        }
        else if (Input.GetButtonUp("Crouch")) StopCrouch();
    }

    void HandleStates()
    {
        // Crouching
        if (Input.GetButton("Crouch"))
        {
            movement_state = MovementState.crouching;
            desired_speed = crouch_speed;
        }
        // Sprinting
        else if (is_grounded && Input.GetButton("Sprint"))
        {
            movement_state = MovementState.sprinting;
            desired_speed = sprint_speed;
            bob_controller.amp_multiplier = 1.5f;
            bob_controller.freq_multiplier = 1.5f;
        }
        // Walking
        else if (is_grounded)
        {
            movement_state = MovementState.walking;
            desired_speed = walk_speed;
            bob_controller.amp_multiplier = 1f;
            bob_controller.freq_multiplier = 1f;
        }
        // Air
        else
        {
            movement_state = MovementState.air;
        }
    }

    // Called every physics frame
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // lerp the movement speed to the desired speed
        movement_speed = Mathf.Lerp(movement_speed, desired_speed, acceleration * Time.fixedDeltaTime);

        float control_multiplier = 1f;
        if (!is_grounded) control_multiplier = air_control;

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

        rb.AddForce(corrected_direction.normalized * movement_speed * rb.mass * 10f * control_multiplier, ForceMode.Force);

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

    void Jump()
    {
        // Reset y-velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jump_force, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        can_jump = true;
    }

    public void Crouch()
    {
        capsule.height = crouch_height;
        rb.AddForce(-transform.up * 500f, ForceMode.Impulse);
    }

    public void StopCrouch()
    {
        capsule.height = player_height;
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
}
