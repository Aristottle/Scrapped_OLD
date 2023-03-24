using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [HideInInspector] public Gun weapon_ref;
    public TMPro.TextMeshProUGUI ammo_text;
    public TMPro.TextMeshProUGUI speed_text;
    public TMPro.TextMeshProUGUI gun_text;
    public GameObject crosshair;
    public Rigidbody rb_ref;

    // Update is called once per frame
    void Update()
    {
        // Ammo
        if (weapon_ref != null)
        {
            int max = weapon_ref.data.mag_size;
            int current = weapon_ref.curr_ammo;
            ammo_text.text = $"{current} / {max}";
            gun_text.text = $"{weapon_ref.data.weapon_name}";
        }

        // Calc the speed
        Vector2 flat_speed = new Vector2(rb_ref.velocity.x, rb_ref.velocity.z);
        int speed = Mathf.RoundToInt(flat_speed.magnitude * 3.6f);
        // Set the text
        speed_text.text = $"{speed} kph";
    }

    public void ToggleCrosshair(bool active)
    {
        crosshair.SetActive(active);
    }
}
