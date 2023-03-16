using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class Gun : MonoBehaviour
{
    [SerializeField] WeaponData data;
    [SerializeField] Transform muzzle;
    [SerializeField] PlayerController wielder;

    Shaker camera_shaker;
    AudioSource sfx_source;
    float time_since_last_shot;
    Transform aim_transform;

    [SerializeField] bool show_debug = false;

    private void Awake()
    {
        // Top off at game start
        data.curr_ammo = data.mag_size;
    }

    private void Start()
    {
        InputHandler.primary_input += Fire;
        InputHandler.reload_input += StartReload;

        aim_transform = wielder.eyes;

        camera_shaker = wielder.camera_fx.camera_shaker;
        sfx_source = GetComponent<AudioSource>();

        // Play the equip sfx
        sfx_source.PlayOneShot(data.equip_sfx);
    }

    private void Update()
    {
        time_since_last_shot += Time.deltaTime;
    }

    private bool CanFire() => !data.is_reloading && time_since_last_shot > 1f / (data.fire_rate / 60f);

    public void Fire()
    {
        // Missfire
        if (data.curr_ammo == 0)
        {
            return;
        }
        // Can fire?
        if (!CanFire())
            return;

        // Ok, we can fire.
        // Calculate spread
        float spread = data.spread / 10;
        Vector2 spread_area = new Vector2(Random.Range(-spread, spread), Random.Range(-spread, spread));
        Vector3 direction = aim_transform.forward + new Vector3(spread_area.x, spread_area.y, 0);
        if (Physics.Raycast(aim_transform.position, direction, out RaycastHit hit_info, data.max_range))
        {
            if (show_debug) 
            {
                Debug.Log($"Hit {hit_info.transform.name}");
                Debug.DrawRay(aim_transform.position, direction * hit_info.distance, Color.green, .5f);
                Debug.DrawRay(muzzle.position, (hit_info.point - muzzle.position) * Vector3.Distance(hit_info.point, muzzle.position), Color.red, .5f);
            }
            IDamageable damageable = hit_info.transform.GetComponent<IDamageable>();
            damageable?.Damage(data.base_damage);
        }

        data.curr_ammo--;
        time_since_last_shot = 0;
        OnFired();
    }

    private void OnFired()
    {
        camera_shaker?.Shake(data.camera_shake);

        // Play the fire sound effect
        sfx_source.PlayOneShot(data.fire_sfx);

        return;
    }

    public void StartReload()
    {
        if (data.is_reloading || data.curr_ammo == data.mag_size)
            return;

        StartCoroutine(Reload());

        // Play the reload sfx
        sfx_source.PlayOneShot(data.reload_sfx);
    }

    private IEnumerator Reload() {
        data.is_reloading = true;

        yield return new WaitForSeconds(data.reload_time);

        data.curr_ammo = data.mag_size;

        data.is_reloading = false;
    }
}
