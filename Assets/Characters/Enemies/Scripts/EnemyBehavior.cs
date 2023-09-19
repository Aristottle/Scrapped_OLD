using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UltEvents;

public class EnemyBehavior : MonoBehaviour
{
    public enum BehaviorState
    {
        Idle,
        Chasing,
        Attacking,
        Entry,
    }

    public UltEvent<BehaviorState> onBehaviorStateChanged;
    
    [SerializeField] BehaviorState defaultBehavior = BehaviorState.Idle;

    private Transform currentTarget;

    private BehaviorState currentBehavior = BehaviorState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        currentBehavior = defaultBehavior;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentBehavior)
        {
            case BehaviorState.Chasing:
                _ChaseUpdate();
                break;
            case BehaviorState.Attacking:
                _AttackUpdate();
                break;
            case BehaviorState.Entry:
                _EntryUpdate();
                break;
            default:
                _IdleUpdate();
                break;
        }

        // Debug print
        // Debug.Log($"Current State: {currentBehavior.ToString()}");
    }

    #region States

    void _IdleUpdate()
    {
        Transform nearestPlayer = GetNearestPlayer();
        if (nearestPlayer != null)
        {
            currentTarget = nearestPlayer;
            ChangeState(BehaviorState.Chasing);
        }
        else
        {
            Debug.Log("ERR: Nearest player == null");
        }
    }
    
    void _ChaseUpdate()
    {
        
    }

    void _AttackUpdate()
    {

    }

    void _EntryUpdate()
    {

    }

    #endregion

    #region Private Helpers

    Transform GetNearestPlayer()
    {
        List<GameObject> allPlayers = PlayerManager.GetPlayerCharacters();

        Transform nearestTransform = null;

        float shortestDistance = -1;
        foreach (var character in allPlayers)
        {
            if (character == null)
            {
                Debug.Log("ERR: Null character from GetPlayerCharacters()");
                continue;
            }

            float distance = Vector3.Distance(transform.position, character.transform.position);
            if (distance <= shortestDistance || shortestDistance < 0)
            {
                nearestTransform = character.transform;
            }
        }

        return nearestTransform;
    }

    private void ChangeState(BehaviorState newState)
    {
        currentBehavior = newState;
        onBehaviorStateChanged?.Invoke(newState);
        Debug.Log($"State changed to {newState}");
    }

    #endregion
}
