using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [HideInInspector] public Gun weapon_ref;
    public TMPro.TextMeshProUGUI ammo_text;
    public TMPro.TextMeshProUGUI speed_text;
    public RectTransform speedometer_bar;
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
        int speed_num = Mathf.CeilToInt(Mathf.Clamp(flat_speed.magnitude * 3.6f, 0, 50));
        float speed_scale = Mathf.Clamp01(speed_num / 50f);
        // Set the text
        speed_text.text = string.Format("{0:00}<size=12> kph</size>", speed_num);
        // Set the scale
        speedometer_bar.localScale = Vector3.Lerp(speedometer_bar.localScale, new Vector3(1, speed_scale, 1), Time.deltaTime * 8);
    }

    public void ToggleCrosshair(bool active)
    {
        crosshair.SetActive(active);
    }
}
