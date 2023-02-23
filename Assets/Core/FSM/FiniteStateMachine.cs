using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField] bool print_debug = false;

    State current_state;

    [HideInInspector] protected Dictionary<string, State> states = new Dictionary<string, State>();

    // Start is called before the first frame update
    private void Start()
    {
        current_state = GetInitialState();
        if (current_state != null) current_state.Enter();
    }

    // Update is called once per frame
    private void Update()
    {
        // Pass the logic update to the current state, if valid
        if (current_state != null) current_state.UpdateLogic();
    }

    private void FixedUpdate()
    {
        // Pass the physics logic to the current state
        if (current_state != null) current_state.UpdatePhysics();
    }

    // Called from the states to transition
    public void TransitionTo(string state_name, Dictionary<string, string> msg = null)
    {
        current_state.Exit();
        current_state = states[state_name];
        current_state.Enter(msg);

        if (print_debug)
        {
            Debug.Log($"Transitioned to {current_state.state_name}");
        }
    }

    // Override to set initial state
    protected virtual State GetInitialState()
    {
        return null;
    }

    // Debugging
    private void OnGUI()
    {
        if (!print_debug) return;
        string content = current_state != null ? current_state.state_name : "NULL";
        GUILayout.Label($"<color='black'><size=24>Current State: {content}</size></color>");
    }
}
