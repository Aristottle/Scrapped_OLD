using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform eyes;
    private Rigidbody rb;
    private PlayerMovement_Old movement;

    [Header("Sliding")]
    [SerializeField] float max_slide_time = 1f;
    [SerializeField] float slide_force = 200f;
    private float slide_timer;

    [Header("Input")]
    private float h_movement;
    private float v_movement;

    private bool is_sliding;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement_Old>();
    }

    private void Update() {
        // Get movement input
        h_movement = Input.GetAxisRaw("Horizontal");
        v_movement = Input.GetAxisRaw("Vertical");

        // Start slide if...
        if (Input.GetButtonDown("Crouch") && Input.GetButton("Sprint") && (h_movement != 0 || v_movement != 0)) StartSlide();
        // Stop slide if...
        if ((Input.GetButtonDown("Sprint") || Input.GetButtonUp("Crouch")) && is_sliding) StopSlide();
    }

    private void FixedUpdate() {
        if (is_sliding) SlidingMovement();
    }

    private void StartSlide()
    {
        is_sliding = true;

        // The crouch function just shrinks the capsule and applies a force to keep the player on the ground
        movement.Crouch();
        
        slide_timer = max_slide_time;
    }
    
    private void SlidingMovement()
    {
        Vector3 input_direction = transform.forward * v_movement + transform.right * h_movement;

        rb.AddForce(input_direction.normalized * slide_force, ForceMode.Force);

        slide_timer -= Time.fixedDeltaTime;

        if (slide_timer <= 0) StopSlide();
    }

    private void StopSlide()
    {
        is_sliding = false;
        movement.StopCrouch();
    }
}
