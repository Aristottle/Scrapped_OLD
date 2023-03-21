using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
    semi_auto,
    full_auto,
    burst,
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData")]

public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weapon_name;
    public GameObject prefab;

    [Header("Firing")]
    public FireMode fire_mode = FireMode.semi_auto;
    public float base_damage = 25;
    public float crit_multi = 1.5f;
    public float max_range = 100f;
    public float spread = .5f;
    public MilkShake.ShakePreset camera_shake;

    [Header("Handling")]
    public bool requires_two_hands = true;
    public float ads_speed = .3f;
    public float ads_forward_offset = .1f;

    [Header("Recoil")]
    public Vector3 recoil_amount = new Vector3(-2, 2, 6);
    public float kickback = .1f;
    public float max_kickback = .2f;
    public float recoil_strength = 5;
    public float return_strength = 8;

    [Header("Ammo / Reloading")]
    public bool can_chamber = true;
    public int mag_size;
    public float fire_rate;
    public float reload_time;

    [Header("SFX")]
    public AudioClip fire_sfx;
    public AudioClip reload_sfx;
    public AudioClip equip_sfx;
    public AudioClip unequip_sfx;
    public AudioClip click_sfx;
}
