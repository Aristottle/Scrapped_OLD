using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{

    #region Variables

    [Header("References")]
    public WeaponData data;
    [SerializeField] Transform muzzle;
    [SerializeField] PlayerController wielder;
    [SerializeField] VisualEffect muzzle_flash;
    Shaker camera_shaker;
    AudioSource sfx_source;
    Transform aim_transform;
    ProceduralRecoil recoil;

    [Header("Runtime Data")]
    bool reloading = false;
    public int curr_ammo {get; private set;} = 0;

    [Header("Control")]
    // This is essentially an input buffer for shooting
    [SerializeField] float input_buffer_duration = .1f;
    bool fire_queued = false;
    float time_since_last_shot;

    [Header("Debug")]
    [SerializeField] bool show_debug = false;

    #endregion


    #region Mono Callbacks

    private void Awake()
    {
        // Top off at game start
        curr_ammo = data.mag_size;
    }

    private void Start()
    {
        // Bind actions
        InputHandler.primary_input += Fire;
        InputHandler.reload_input += StartReload;

        // Get references
        aim_transform = wielder.eyes;
        camera_shaker = wielder.camera_fx.camera_shaker;
        sfx_source = GetComponent<AudioSource>();

        // Initialize recoil
        recoil = GetComponent<ProceduralRecoil>();

        // Initialize the procedural animation controller
        ProceduralWeaponAnimation weapon_anims = GetComponent<ProceduralWeaponAnimation>();
        weapon_anims?.Init(wielder);

        // Initialize ADS. Have to do it here because we need the transform component to be initialized.
        GetComponent<ProceduralADS>()?.Init(data.ads_speed, data.ads_forward_offset, transform, aim_transform, weapon_anims);

        // Play the equip sfx
        sfx_source.PlayOneShot(data.equip_sfx);
    }

    private void Update()
    {
        time_since_last_shot += Time.deltaTime;

        if (fire_queued && CanFire())
        {
            Fire();
        }
    }

    private void OnDestroy() 
    {
        InputHandler.primary_input -= Fire;
        InputHandler.reload_input -= StartReload;
    }

    #endregion


    #region Private Methods

    private bool CanFire() => !reloading && time_since_last_shot > 1f / (data.fire_rate / 60f);

    private void OnFired()
    {
        // Particle
        muzzle_flash.Play();

        camera_shaker?.Shake(data.camera_shake);

        // Play the fire sound effect
        sfx_source.PlayOneShot(data.fire_sfx);

        // Apply recoil
        recoil?.AddRecoil();

        return;
    }

    private IEnumerator Reload() {
        reloading = true;

        yield return new WaitForSeconds(data.reload_time);

        int new_ammo_count = data.mag_size;
        if (curr_ammo > 0 && data.can_chamber)
            new_ammo_count += 1;

        curr_ammo = new_ammo_count;

        reloading = false;
    }

    private void CancelQueuedShot()
    {
        fire_queued = false;
        return;
    }

    #endregion


    #region Public Methods

    public void Init(PlayerController w)
    {
        wielder = w;
    }

    public void Fire()
    {
        // Can fire?
        if (!CanFire())
        {
            // Queue a shot
            fire_queued = true;
            Invoke(nameof(CancelQueuedShot), input_buffer_duration);
            return;
        }

        // Ok, we can fire.
        fire_queued = false;
        CancelInvoke(nameof(CancelQueuedShot));
        
        // Missfire
        if (curr_ammo == 0)
        {
            sfx_source.PlayOneShot(data.click_sfx, .6f);
            return;
        }
       
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

        curr_ammo--;
        time_since_last_shot = 0;
        OnFired();
    }

    public void StartReload()
    {
        if (reloading || curr_ammo >= data.mag_size)
            return;

        StartCoroutine(Reload());

        // Play the reload sfx
        sfx_source.PlayOneShot(data.reload_sfx, .6f);
    }

    #endregion

}
