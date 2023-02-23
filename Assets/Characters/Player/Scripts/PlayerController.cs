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
    private float desired_speed;
    public float walk_speed = 5f;
    public float sprint_speed = 8f;
    public float crouch_speed = 3f;
    public float wallrun_speed = 10f;
    public float acceleration = 10f;

    [Header("Crouching")]
    public float standing_height = 2f;
    public float crouch_height = 1f;

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
    public Rigidbody rb;
    public HeadBobController bob_controller;

    [HideInInspector] public bool is_grounded;


    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        bob_controller = GetComponent<HeadBobController>();

        standing_height = capsule.height;
    }

    private void Update() 
    {
        // Keep track of whether we're grounded
        ground_check = new Vector3(capsule.transform.position.x, capsule.transform.position.y - capsule.height / 2, capsule.transform.position.z);
        is_grounded = Physics.CheckSphere(ground_check, ground_check_radius, ground_mask);
    }
}
