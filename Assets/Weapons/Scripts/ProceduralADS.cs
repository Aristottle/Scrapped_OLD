using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralADS : MonoBehaviour
{
    #region Variables

    [Header("Data")]
    float ads_speed;
    float ads_z_offset;

    [Header("Runtime Data")]
    bool can_ads = false;
    bool is_ads = false;
    Vector3 target_pos, origin_pos;

    [Header("References")]
    [SerializeField] Transform ads_transform;
    Transform weapon_transform;
    Transform camera_transform;
    ProceduralWeaponAnimation proc_anims;

    [Header("Debugging")]
    [SerializeField] bool debug_stuff;

    #endregion


    #region Mono Callbacks

    private void Update() 
    {
        if (debug_stuff)
        {
            DebugExtension.DebugWireSphere(weapon_transform.position, Color.red, .01f, Time.deltaTime);
            // This is where the gun should be when aiming
            DebugExtension.DebugWireSphere(weapon_transform.parent.position + target_pos, Color.magenta, .01f, Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        ADSUpdate();    
    }

    private void OnDestroy() 
    {
        InputHandler.alternate_input -= ToggleADS;
    }

    #endregion


    #region Private Methods

    private void UpdateTargetPoint()
    {
        // Gets the camera_transform relative to the weapon_transform
        Vector3 relative_cam_pos = camera_transform.InverseTransformPoint(weapon_transform.position);
        // Gets the ads_transform relative to the weapon_transform
        Vector3 relative_ads_pos = ads_transform.InverseTransformPoint(weapon_transform.position);
        // Calculate the sight_offset (how to move the weapon)
        Vector3 sight_offset = relative_cam_pos - relative_ads_pos;
        if (debug_stuff) Debug.Log(sight_offset);
        // Set the new target_pos
        target_pos = origin_pos - sight_offset;
        target_pos.z += ads_z_offset;
    }

    private void ToggleADS()
    {
        if (is_ads)
            StopADS();
        else StartADS();
    }

    private void StartADS()
    {
        proc_anims?.TogglePlay(false);
        is_ads = true;
        UpdateTargetPoint();
    }

    private void StopADS()
    {
        proc_anims?.TogglePlay(true);
        is_ads = false;
        target_pos = origin_pos;
    }

    // This is the only thing called on Update()
    private void ADSUpdate()
    {
        // Only run this code if necessary
        if (!can_ads && weapon_transform.localPosition == target_pos)
            return;

        // if (is_ads)
        //     UpdateTargetPoint();

        weapon_transform.localPosition = Vector3.Lerp(weapon_transform.localPosition, target_pos, Time.deltaTime / ads_speed);
    }

    #endregion


    #region Public Methods

    // Called externally to initialize the component
    public void Init(float ads_speed, float z_offset, Transform weapon, Transform cam, ProceduralWeaponAnimation anim_controller)
    {
        this.ads_speed = ads_speed;
        this.weapon_transform = weapon;
        this.camera_transform = cam;
        this.ads_z_offset = z_offset;

        origin_pos = weapon_transform.localPosition;
        target_pos = origin_pos;

        proc_anims = anim_controller;

        // Bind toggle_ads to input
        InputHandler.alternate_input += ToggleADS;
    }

    public void SetADSEnabled(bool enable)
    {
        can_ads = enable;

        if (enable)
            InputHandler.alternate_input += ToggleADS;
        else
            InputHandler.alternate_input -= ToggleADS;
    }

    #endregion

}
