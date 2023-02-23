using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    [SerializeField] LayerMask world_mask;
    [SerializeField] float wallrun_force;
    [SerializeField] float max_wr_time;
    private float wr_timer;

    [Header("Input")]
    private float h_movement;
    private float v_movement;

    [Header("Detection")]
    [SerializeField] float wall_check_distance = .5f;
    [SerializeField] float min_jump_height = 1f;
    private RaycastHit left_hit;
    private RaycastHit right_hit;
    private bool wall_right;
    private bool wall_left;

    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] Transform camera_ref;
    private PlayerMovement movement_ref;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        movement_ref = GetComponent<PlayerMovement>();
    }

    private void Update() {
        WallCheck();
    }

    private void WallCheck()
    {
        float distance = wall_check_distance + movement_ref.capsule.radius;

        wall_right = Physics.Raycast(transform.position, transform.right, out right_hit, distance, world_mask);
        wall_left = Physics.Raycast(transform.position, -transform.right, out left_hit, distance, world_mask);
    }

    private bool CanWallrun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, min_jump_height + movement_ref.capsule.height / 2, world_mask);
    }

    private void FSM()
    {
        // Get movement input
        h_movement = Input.GetAxisRaw("Horizontal");
        v_movement = Input.GetAxisRaw("Vertical");

        // Wallrunning state
        if ((wall_left || wall_right) && v_movement > 0 && CanWallrun())
        {
            // Start wallrun

        }
    }

    private void StartWallrun()
    {
        
    }

    private void WallrunMovement()
    {
        
    }
    
    private void StopWallrun()
    {

    }
}
