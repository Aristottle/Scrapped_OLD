using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData")]

public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weapon_name;

    [Header("Firing")]
    public float base_damage = 25;
    public float crit_multi = 1.5f;
    public float max_range = 100f;
    public float spread = .5f;
    public MilkShake.ShakePreset camera_shake;

    [Header("Ammo / Reloading")]
    public int curr_ammo;
    public int mag_size;
    public float fire_rate;
    public float reload_time;
    [HideInInspector] public bool is_reloading;

    [Header("SFX")]
    public AudioClip fire_sfx;
    public AudioClip reload_sfx;
    public AudioClip equip_sfx;
    public AudioClip unequip_sfx;
}
