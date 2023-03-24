using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    #region Variables

    [Header("Defaults")]
    [SerializeField] WeaponData default_main;
    [SerializeField] WeaponData default_alt;

    [Header("Config")]
    [SerializeField] Dictionary<string, Transform> sockets;
    [SerializeField] Transform main, alternate;

    [Header("References")]
    [SerializeField] PlayerController player;
    public PlayerHUD hud_manager;
    private Dictionary<string, Gun> weapon_slots;

    [Header("PowerUps")]
    public bool infinite_ammo = false;
    public bool insta_kill = false;

    #endregion


    #region Mono Callbacks

    private void Awake() 
    {
        sockets = new Dictionary<string, Transform>();
        weapon_slots = new Dictionary<string, Gun>();

        // Initialize the sockets. Unity doesn't let us serialize dictionaries so have to do it in code.
        sockets.Add("Main", main);
        sockets.Add("Alternate", alternate);
        // Init slots.
        weapon_slots.Add("Main", null);
        weapon_slots.Add("Alternate", null);
    }

    private void Start() 
    {
        EquipInitialWeapons();
    }
    
    #endregion


    #region Private Methods

    private void EquipInitialWeapons()
    {
        if (default_main != null)
            EquipWeapon(default_main, "Main");
        if (default_alt != null)
            EquipWeapon(default_alt, "Alternate");
    }
    
    #endregion


    #region Public Methods

    public bool EquipWeapon(WeaponData weapon, string slot_name)
    {
        if (weapon == null)
        {
            Debug.Log("Invalid weapon data tried to be equipped.");
            return false;
        }

        if (!sockets.ContainsKey(slot_name))
        {
            Debug.Log("Invalid slot_name in WeaponManager.");
            return false;
        }

        // Make sure we're not equipping the same weapon
        if (weapon_slots[slot_name] != null)
            if (weapon_slots[slot_name].data == weapon)
                return false;
        
        // Unequip whatever is currently there
        if (weapon_slots[slot_name] != null)
            Destroy(weapon_slots[slot_name].gameObject);
        weapon_slots[slot_name] = null;

        // Create the new gun
        Gun new_gun = Instantiate(weapon.prefab, sockets[slot_name]).GetComponent<Gun>();
        new_gun.Init(player, this);
        weapon_slots[slot_name] = new_gun;

        // Update the HUD
        hud_manager.weapon_ref = new_gun;

        return true;
    }

    #endregion

}
