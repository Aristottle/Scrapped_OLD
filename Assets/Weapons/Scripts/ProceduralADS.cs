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
        DebugExtension.DebugWireSphere(weapon_transform.position, Color.red, .01f, Time.deltaTime);
        // Debug.Log(camera_transform.forward);
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
        Vector3 sight_offset = camera_transform.position - ads_transform.position;
        Debug.Log(sight_offset);
        target_pos = origin_pos + sight_offset;
        target_pos += camera_transform.forward * ads_z_offset;

        // This is where the gun should be when aiming
        DebugExtension.DebugWireSphere(transform.position + target_pos, Color.magenta, .01f, 1);
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
