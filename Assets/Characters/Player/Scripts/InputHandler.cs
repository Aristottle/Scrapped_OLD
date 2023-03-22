using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    #region Variables

    public static Action primary_input;
    public static Action reload_input;
    public static Action alternate_input;

    [Header("Debugging")]
    [SerializeField] WeaponManager weapon_manager;
    [SerializeField] WeaponData gun_one;
    [SerializeField] WeaponData gun_two;
    [SerializeField] WeaponData gun_three;
    [SerializeField] WeaponData gun_four;
    [SerializeField] WeaponData gun_five;
    [SerializeField] WeaponData gun_six;

    #endregion


    #region Mono Callbacks

    // Update is called once per frame
    void Update()
    {
        if (UIManager.game_paused)
            return;

        // Shoot Input
        if (Input.GetButtonDown("Primary Action"))
            primary_input?.Invoke();
        
        // ADS Input
        if (Input.GetButtonDown("Alternate Action"))
            alternate_input?.Invoke();

        // Reload Input
        if (Input.GetButtonDown("Reload"))
            reload_input?.Invoke();

        DebugUpdate();
    }

    #endregion


    #region Private Methods

    private void DebugUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon_manager.EquipWeapon(gun_one, "Main");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon_manager.EquipWeapon(gun_two, "Main");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon_manager.EquipWeapon(gun_three, "Main");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weapon_manager.EquipWeapon(gun_four, "Main");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weapon_manager.EquipWeapon(gun_five, "Main");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            weapon_manager.EquipWeapon(gun_six, "Main");
        }
    }

    #endregion

}
