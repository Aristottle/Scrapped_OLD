using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{

    #region Variables

    [Header("Head Bob")]
    public bool active = true;
    [SerializeField] float amplitude = .015f;
    [SerializeField] float frequency = 10f;
    [SerializeField] Transform _camera = null;
    [SerializeField] Transform _camera_root = null;

    [HideInInspector] public float amp_multiplier = 1f;
    [HideInInspector] public float freq_multiplier = 1f;

    private float toggle_speed = 1.25f;
    private Vector3 start_position;
    private Rigidbody rb;

    #endregion


    #region MonoBehaviour Callbacks

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        start_position = _camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) 
        {
            ResetPosition();
            return;
        }

        CheckMotion();
        // Stabilization
        // Vector3 stabilized_rotation = Quaternion.LookRotation((FocusTarget() - _camera.position).normalized, Vector3.up).eulerAngles;
        // _camera.localRotation = Quaternion.Euler(stabilized_rotation.x, stabilized_rotation.y, _camera.localEulerAngles.z);
    }

    #endregion


    #region Private Methods

    private void CheckMotion()
    {
        float speed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;

        ResetPosition();

        if (speed < toggle_speed) return;
        // if (!movement.is_grounded) return;

        _camera.localPosition += CameraMotion();
    }

    private Vector3 CameraMotion()
    {
        Vector3 pos = Vector3.zero;
        float fixed_amp = (amplitude / 100) * amp_multiplier;
        float fixed_freq = frequency * freq_multiplier;
        pos.y += Mathf.Sin(Time.time * fixed_freq) * fixed_amp;
        pos.x += Mathf.Cos(Time.time * fixed_freq / 2) * fixed_amp * 2;
        return pos;
    }

    private void ResetPosition()
    {
        if (_camera.localPosition == start_position) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, start_position, 5 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(_camera_root.transform.position.x, _camera_root.transform.position.y, _camera_root.transform.position.z);
        pos += _camera_root.forward * 15f;
        return pos;
    }

    #endregion

}
