using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// This class manages all of the camera effects. Headbob, tilt, fov, shake, etc.
/// </summary>

public class CameraEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] HeadBobController bob_controller;
    [SerializeField] Camera camera_ref;

    float base_fov;

    private void Start() 
    {
        base_fov = camera_ref.fieldOfView;
    }

    /// <summary>
    ///     HEAD BOB
    /// </summary>

    public void ToggleHeadBob(bool enable)
    {
        bob_controller.enabled = enable;
    }

    public void SetHeadBobMultipliers(float amp_multiplier = 1f, float freq_multiplier = 1f)
    {
        bob_controller.amp_multiplier = amp_multiplier;
        bob_controller.freq_multiplier = freq_multiplier;
    }

    /// <summary>
    ///     CAMERA TILT AND FOV
    /// </summary>

    public void TiltCamera(float tilt, float time = .3f)
    {
        // camera_ref.transform.DOLocalRotate(new Vector3(0, 0, tilt), time);
    }

    public void OffsetFOV(float offset, float time = .3f)
    {
        camera_ref.DOFieldOfView(base_fov + offset, time);
    }
}
