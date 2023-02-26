using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

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

    [Header("Sliding")]
    public float slide_impulse = 300f;
    public float slide_direction_influence = .5f;

    [Header("Wallrunning")]
    public float wall_check_distance = .5f;
    public float min_jump_height = 1.5f;
    public float wallrun_gravity = 1.5f;
    public float wallrun_movement_force = 200f;
    public float wall_jump_v_force = 400f;
    public float wall_jump_h_force = 200f;
    public float max_wallrun_time = 5f;
    [HideInInspector] public bool wall_left = false;
    [HideInInspector] public bool wall_right = false;
    [HideInInspector] public RaycastHit wall_hit_right;
    [HideInInspector] public RaycastHit wall_hit_left;

    [Header("Drag")]
    [HideInInspector] public float drag;
    public float ground_drag {get {return 8f;}}
    public float wallrun_drag {get {return 2f;}}
    public float air_drag {get {return 0f;}}
    public float slide_drag {get {return 1f;}}

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
    public CameraEffects camera_fx;

    [HideInInspector] public bool is_grounded = true;
    bool do_ground_check = true;

    Vector3 movement_direction;

    float base_fov;


    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

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

        // Keep the velocity clamped
        ClampVelocity();
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

    /// <summary>
    /// Slope Handling
    /// </summary>
    /// <returns></returns>

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

    /// <summary>
    /// Crouching logic
    /// </summary>

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

    public bool CanStand()
    {
        Vector3 pos = new Vector3(transform.position.x, (capsule.transform.position.y - capsule.height / 2) + standing_height - capsule.radius, transform.position.z);
        return !Physics.CheckSphere(pos, capsule.radius, ground_mask);
    }

    /// <summary>
    /// Ground check control
    /// </summary>
    /// <param name="duration"></param>

    private void PauseGroundCheck(float duration)
    {
        do_ground_check = false;
        Invoke(nameof(ResumeGroundCheck), duration);
    }

    private void ResumeGroundCheck()
    {
        do_ground_check = true;
    }

    /// <summary>
    /// Wall check logic. Called from the air and wallrun states in their update_logic()
    /// </summary>

    public void CheckWall()
    {
        wall_left = Physics.Raycast(transform.position, -transform.right, out wall_hit_left, capsule.radius + wall_check_distance);
        wall_right = Physics.Raycast(transform.position, transform.right, out wall_hit_right, capsule.radius + wall_check_distance);
    }

    public bool CanWallrun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, (capsule.height / 2f) + min_jump_height, ground_mask) && (GetMovementInput().y >= 0.1f);
    }

    public void Wallrun()
    {
        rb.useGravity = false;

        Vector3 wall_normal = wall_right ? wall_hit_right.normal : wall_hit_left.normal;

        Vector3 wall_forward = Vector3.Cross(wall_normal, transform.up);

        // Move in the direction we're facing
        if ((transform.forward - wall_forward).magnitude > (transform.forward - -wall_forward).magnitude)
            wall_forward *= -1;

        // Wallrun movement
        rb.AddForce(wall_forward * wallrun_movement_force, ForceMode.Force);

        // Wall adherance
        if (!(wall_left && GetMovementInput().x > 0) && !(wall_right && GetMovementInput().x < 0))
            rb.AddForce(-wall_normal * 100f, ForceMode.Force);

        // Wallrun gravity
        rb.AddForce(Vector3.down * wallrun_gravity, ForceMode.Force);
    }
}
