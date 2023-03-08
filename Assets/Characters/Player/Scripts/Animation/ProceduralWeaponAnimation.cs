using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWeaponAnimation : MonoBehaviour
{
    #region Variables

    [Header("Rotation")]
    public float rotation_intensity = 10f;
    public float rotation_adherance = 5f;
    public float max_tilt = 10f; // In degrees

    [Header("Position")]
    public float position_intensity = 10f;
    public float position_adherance = 5f;
    public float max_position_offset = .025f; // In meters

    private Quaternion origin_rotation;
    private Vector3 origin_position;

    #endregion



    #region MonoBehaviour Callbacks

    private void Start()
    {
        origin_rotation = transform.localRotation;
        origin_position = transform.localPosition;
    }

    private void Update()
    {
        UpdateSway();
    }

    #endregion



    #region Private Methods

    private void UpdateSway()
    {
        Vector2 look_input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Calculate position offset
        float position_offset_x = Mathf.Clamp(-look_input.x * position_intensity, -max_position_offset, max_position_offset);
        float position_offset_y = Mathf.Clamp(-look_input.y * position_intensity, -max_position_offset, max_position_offset);
        Vector3 target_position = new Vector3(position_offset_x, position_offset_y, 0);

        // Move to target position
        transform.localPosition = Vector3.Lerp(transform.localPosition, target_position + origin_position, Time.deltaTime * position_adherance);

        // Calculate target rotation
        Quaternion yaw_offset = Quaternion.AngleAxis(rotation_intensity * -look_input.x, Vector3.up);
        Quaternion pitch_offset = Quaternion.AngleAxis(rotation_intensity * look_input.y, Vector3.right);
        Quaternion roll_offset = Quaternion.AngleAxis(Mathf.Clamp(rotation_intensity * 2f * -look_input.x, 0f, max_tilt), Vector3.forward);
        Quaternion target_rotation = origin_rotation * yaw_offset * pitch_offset * roll_offset;

        // Rotate to target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target_rotation, rotation_adherance * Time.deltaTime);
    }

    #endregion
}
