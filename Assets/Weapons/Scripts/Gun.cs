using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] WeaponData data;
    [SerializeField] Transform muzzle;

    float time_since_last_shot;

    [SerializeField] bool show_debug = false;

    private void Start()
    {
        InputHandler.primary_input += Fire;
        InputHandler.reload_input += StartReload;
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
        if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit_info, data.max_range))
        {
            if (show_debug) Debug.Log($"Hit {hit_info.transform.name}");
            IDamageable damageable = hit_info.transform.GetComponent<IDamageable>();
            damageable?.Damage(data.base_damage);
        }

        data.curr_ammo--;
        time_since_last_shot = 0;
        OnFired();
    }

    private void OnFired()
    {
        return;
    }

    public void StartReload()
    {
        if (data.is_reloading)
            return;

        StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        data.is_reloading = true;

        yield return new WaitForSeconds(data.reload_time);

        data.curr_ammo = data.mag_size;

        data.is_reloading = false;
    }
}
