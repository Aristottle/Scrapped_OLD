using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    [Header("Checkpoints")]
    [SerializeField] TrialCheckpoint start;
    [SerializeField] TrialCheckpoint end;

    [Header("Time")]
    private float best_time = -1;

    private HUDTimer hud_timer;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the overlap events of the triggers
        TrialCheckpoint.PlayerEntered += PlayerDetected;
    }

    void PlayerDetected(bool start, GameObject player)
    {
        hud_timer = player.transform.parent.GetComponentInChildren<HUDTimer>();

        if (start)
            StartTrial();
        else
            EndTrial();
    }
    
    void StartTrial()
    {
        hud_timer.StartTimer();
    }

    void EndTrial()
    {
        float run_time = hud_timer.StopTimer();

        if (run_time < best_time || best_time == -1)
        {
            best_time = run_time;
            Debug.Log($"New best time of {best_time}!");
        }
        else
        {
            Debug.Log("Didn't beat PB.");
        }
    }
}
