using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public Transform player_transform;

    [SerializeField] float position_lerp_speed = 6f;

    [SerializeField] PlayerControlsData control_settings;

    [Header("References")]
    [SerializeField] CapsuleCollider capsule;

    float mouse_x;
    float mouse_y;
    float x_rot;
    float y_rot;
    
    Vector3 standing_pos;
    float player_height;

    float look_multi = 2f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        standing_pos = transform.localPosition;      
        player_height = capsule.height;  
    }

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked) HandleInput();

        x_rot = Mathf.Clamp(x_rot, -87f, 87f);

        // Apply the rotations
        transform.localRotation = Quaternion.Euler(x_rot, 0, 0);
        player_transform.rotation = Quaternion.Euler(0, y_rot, 0);

        // Lerp the camera's local position
        Vector3 offset = new Vector3(standing_pos.x, standing_pos.y - (player_height - capsule.height) / 2f, standing_pos.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, offset, Time.deltaTime * position_lerp_speed);
    }

    private void HandleInput()
    {
        mouse_x = Input.GetAxisRaw("Mouse X");
        mouse_y = Input.GetAxisRaw("Mouse Y");

        y_rot += mouse_x * control_settings.mouse_sensitivity * look_multi;
        x_rot -= mouse_y * control_settings.mouse_sensitivity * look_multi;
    }

    public void AddLookRotation(Vector2 rotation)
    {
        x_rot += rotation.x;
        y_rot += rotation.y;
    }
}
