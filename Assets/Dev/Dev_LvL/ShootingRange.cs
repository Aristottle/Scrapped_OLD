using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRange : MonoBehaviour
{

    #region Variables

    ShootingRangeTarget[] targets;
    float tick_rate = 5;

    #endregion

    
    #region Mono Callbacks

    void Start()
    {
        targets = GetComponentsInChildren<ShootingRangeTarget>();

        Invoke(nameof(TargetsUpdate), tick_rate);
    }

    #endregion


    #region Private Methods

    private void TargetsUpdate()
    {
        bool all_destroyed = true;
        foreach (ShootingRangeTarget target in targets)
        {
            if (!target.destroyed)
                all_destroyed = false;
        }

        if (all_destroyed)
        {
            foreach (ShootingRangeTarget target in targets)
                target.ToggleHidden(false);
        }

        Invoke(nameof(TargetsUpdate), tick_rate);
    }

    #endregion

}
