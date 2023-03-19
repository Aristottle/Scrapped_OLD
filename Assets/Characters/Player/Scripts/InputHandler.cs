using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    #region Variables

    public static Action primary_input;
    public static Action reload_input;

    [Header("Debugging")]
    [SerializeField] WeaponManager weapon_manager;
    [SerializeField] WeaponData gun_one;
    [SerializeField] WeaponData gun_two;

    #endregion


    #region Mono Callbacks

    // Update is called once per frame
    void Update()
    {
        // Shoot Input
        if (Input.GetButtonDown("Primary Action"))
            primary_input?.Invoke();

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
    }

    #endregion

}
