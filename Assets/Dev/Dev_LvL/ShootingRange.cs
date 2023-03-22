using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRange : MonoBehaviour
{

    #region Variables

    ShootingRangeTarget[] targets;
    int num_disabled = 0;

    #endregion

    
    #region Mono Callbacks

    void Start()
    {
        targets = GetComponentsInChildren<ShootingRangeTarget>();

        foreach (ShootingRangeTarget t in targets)
        {
            t.OnDeath += TargetsUpdate;
        }
    }

    private void OnDisable() 
    {
        foreach (ShootingRangeTarget t in targets)
        {
            t.OnDeath -= TargetsUpdate;
        }
    }

    #endregion


    #region Private Methods

    private void TargetsUpdate()
    {
        num_disabled++;

        if (num_disabled == targets.Length)
        {
            num_disabled = 0;
            Invoke(nameof(RestoreTargets), 3);
        }
    }

    private void RestoreTargets()
    {
        foreach (ShootingRangeTarget t in targets)
        {
            t.ToggleHidden(false);
        }
    }

    #endregion

}
