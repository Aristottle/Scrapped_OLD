using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWeaponAnimation : MonoBehaviour
{
    #region Variables

    [Header("Animation")]
    public float movement_freq;
    public float movement_intensity;
    public float idle_freq;
    public float idle_intensity;

    [Header("Sway")]
    public float rotation_intensity = 10f;
    public float rotation_adherance = 5f;
    public float max_tilt = 10f; // In degrees
    public float position_intensity = 10f;
    public float position_adherance = 5f;
    public float max_position_offset = .025f; // In meters

    [Header("References")]
    [SerializeField] PlayerController player;
    [SerializeField] Transform anim_root;

    private Quaternion origin_rotation;
    private Vector3 origin_position;
    private Quaternion root_origin_rotation;
    private Vector3 root_origin_position;
    private float idle_time;
    private float movement_time;

    #endregion


    #region MonoBehaviour Callbacks

    private void Start()
    {
        origin_rotation = transform.localRotation;
        origin_position = transform.localPosition;
        root_origin_rotation = anim_root.localRotation;
        root_origin_position = anim_root.localPosition;
    }

    private void Update()
    {
        UpdateSway();
        UpdateAnimation();
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
        Quaternion roll_offset = Quaternion.AngleAxis(Mathf.Clamp(rotation_intensity * 3f * -look_input.x, 0f, max_tilt), Vector3.forward);
        Quaternion target_rotation = origin_rotation * yaw_offset * pitch_offset * roll_offset;

        // Rotate to target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target_rotation, rotation_adherance * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        // Get the player's speed and falling speed
        float speed = new Vector2(player.rb.velocity.x, player.rb.velocity.z).magnitude;
        float falling_speed = player.is_grounded ? player.rb.velocity.y : 0;

        // Idle / Movement
        if (speed > 0.5f && (player.is_grounded || player.is_wallrunning))
        {
            float movement_scale = Mathf.Lerp(1, 2, Mathf.Clamp(speed / player.sprint_speed, 0f, 1f));
            Movement(movement_time, movement_scale);
            movement_time += Time.deltaTime;
        }
        else 
        {
            Idle(idle_time);
            idle_time += Time.deltaTime;
        }

        // TODO: Crouch tilt
    }

    private void Idle(float t)
    {
        float h_motion = Mathf.Cos(t * idle_freq / 2) * (idle_intensity * 2);
        float v_motion = Mathf.Sin(t * idle_freq) * idle_intensity;
        anim_root.localPosition = Vector3.Lerp(anim_root.localPosition, root_origin_position + new Vector3(h_motion, v_motion, 0), Time.deltaTime * 10f);
    }
    
    private void Movement(float t, float scale = 1)
    {
        float freq = movement_freq;
        float h_motion = Mathf.Cos(t * freq / 2) * (movement_intensity * 2 * scale);
        float v_motion = Mathf.Sin(t * freq) * movement_intensity * scale;
        anim_root.localPosition = Vector3.Lerp(anim_root.localPosition, root_origin_position + new Vector3(h_motion, v_motion, 0), Time.deltaTime * 10f);
    }
    
    private void CrouchTilt()
    {
        return;
    }

    #endregion


    #region Public Methods

    public void Init(PlayerController p)
    {
        player = p;
    }

    #endregion

}
