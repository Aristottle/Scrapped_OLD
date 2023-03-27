using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    [Header("Checkpoints")]
    [SerializeField] TrialCheckpoint start;
    [SerializeField] TrialCheckpoint end;

    [Header("Time")]
    private float best_time = 0f;
    private float time_elapsed = 0f;

    private bool running_trial = false;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the overlap events of the triggers
        TrialCheckpoint.PlayerEntered += PlayerDetected;
    }

    // Update is called once per frame
    void Update()
    {
        if (running_trial)
        {
            time_elapsed += Time.deltaTime;        
        }
    }

    void PlayerDetected(bool start)
    {
        if (start)
            StartTrial();
        else
            EndTrial();
            
    }
    
    void StartTrial()
    {
        running_trial = true;
    }

    void EndTrial()
    {
        running_trial = false;

        if (time_elapsed <= best_time || best_time <= 0)
            best_time = time_elapsed;

        time_elapsed = 0f;
    }

    // private void OnGUI()
    // {
    //     double time = (double) time_elapsed;
    //     double pb = (double) best_time;
    //     string content = running_trial ? $"Time: {time}\nTrial PB: {pb}" : $"Trial PB: {pb}";
    //     GUILayout.Label($"<color='black'><size=24>{content}</size></color>");
    // }
}
