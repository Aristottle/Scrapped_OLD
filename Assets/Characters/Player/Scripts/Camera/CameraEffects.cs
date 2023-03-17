using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;
using MilkShake;

/// <summary>
/// This class manages all of the camera effects. Headbob, tilt, fov, shake, etc.
/// </summary>

public class CameraEffects : MonoBehaviour
{

    #region Variables

    [Header("References")]
    [SerializeField] HeadBobController bob_controller;
    [SerializeField] Camera camera_ref;
    [SerializeField] Transform camera_pivot;
    [SerializeField] PlayerController player;
    [Header("Speed Lines")]
    [SerializeField] VisualEffect speed_lines;
    [SerializeField] float speed_lines_toggle_speed = 5.5f;
    [SerializeField] float speed_lines_max_spawn_rate = 100;
    [Header("Shake")]
    public Shaker camera_shaker;

    float base_fov;

    float speed_lines_alpha = 0;

    #endregion


    #region Mono Callbacks

    private void Start() 
    {
        base_fov = camera_ref.fieldOfView;
    }

    private void Update() 
    {
        HandleSpeedLines();
    }

    #endregion


    #region HeadBob

    public void ToggleHeadBob(bool enable)
    {
        bob_controller.active = enable;
    }

    public void SetHeadBobMultipliers(float amp_multiplier = 1f, float freq_multiplier = 1f)
    {
        bob_controller.amp_multiplier = amp_multiplier;
        bob_controller.freq_multiplier = freq_multiplier;
    }

    #endregion


    #region Tilt/FOV

    public void TiltCamera(float tilt, float time = .3f)
    {
        camera_pivot.DOLocalRotate(new Vector3(0, 0, tilt), time);
    }

    public void OffsetFOV(float offset, float time = .3f)
    {
        camera_ref.DOFieldOfView(base_fov + offset, time);
    }

    #endregion

    
    #region Speedlines

    private void HandleSpeedLines()
    {
        float desired_alpha = Mathf.Clamp((player.rb.velocity.magnitude / player.terminal_velocity) - (speed_lines_toggle_speed / player.terminal_velocity), 0, 1);
        speed_lines_alpha = Mathf.Lerp(speed_lines_alpha, desired_alpha, Time.deltaTime * 5f);

        // Set the spawn rate of the 
        speed_lines.SetFloat(Shader.PropertyToID("spawn_rate"), speed_lines_alpha * speed_lines_max_spawn_rate);
    }

    #endregion

}
