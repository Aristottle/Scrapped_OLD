using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] Transform recoil_root;

    [Header("Transforms")]
    Vector3 origin_pos, origin_rot;
    Vector3 target_pos, target_rot;
    Vector3 current_pos, current_rot;

    [Header("Data")]
    Vector3 recoil_amount;
    float kickback, recoil_strength, return_strength;

    #endregion


    #region Mono Callbacks

    private void Start() 
    {
        // Get the recoil data
        WeaponData data = GetComponent<Gun>()?.data;
        recoil_amount = data.recoil_amount;
        kickback = data.kickback;
        recoil_strength = data.recoil_strength;
        return_strength = data.return_strength;

        // Get the initial origins
        origin_pos = recoil_root.localPosition;
        origin_rot = recoil_root.localEulerAngles;
    }

    private void Update()
    {
        UpdateRecoil();
    }

    #endregion


    #region Private Methods

    private void UpdateRecoil()
    {
        // If target is origin (we have no recoil at the moment), do no math
        if (target_pos == origin_pos && target_rot == origin_rot)
            return;

        // Return the target rotation to origin (this is the return of the recoil)
        target_rot = Vector3.Lerp(target_rot, origin_rot, Time.deltaTime * return_strength);
        // Update the current rotation to the target rotation (this adds recoil)
        current_rot = Vector3.Slerp(current_rot, target_rot, Time.fixedDeltaTime * recoil_strength);
        // Set the rotation of the recoil root to the current_rotation
        recoil_root.localRotation = Quaternion.Euler(current_rot);
        // Update the kickback
        UpdateKickback();
    }

    private void UpdateKickback()
    {
        // Return strength
        target_pos = Vector3.Lerp(target_pos, origin_pos, Time.deltaTime * return_strength);
        // Recoil strength
        current_pos = Vector3.Lerp(current_pos, target_pos, Time.fixedDeltaTime * recoil_strength);
        // Apply the kickback
        recoil_root.localPosition = current_pos;
    }

    #endregion


    #region Public Methods

    public void AddRecoil()
    {
        // Kickback
        target_pos -= new Vector3(0, 0, kickback);
        // Recoil
        float rot_x = recoil_amount.x;
        float rot_y = Random.Range(-recoil_amount.y, recoil_amount.y);
        float rot_z = Random.Range(-recoil_amount.z, recoil_amount.z);
        target_rot += new Vector3(rot_x, rot_y, rot_z);
    }

    #endregion

}