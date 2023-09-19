using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private enum BehaviorState
    {
        Idle,
        Chasing,
        Attacking,
        Entry,
    }
    
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
        Debug.Log($"Current State: {currentBehavior.ToString()}");
    }

    #region States

    void _IdleUpdate()
    {
        if (GetNearestPlayer() != null)
        {
            currentTarget = GetNearestPlayer();
            currentBehavior = BehaviorState.Chasing;
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
        bool changed = false;

        float shortestDistance = -1;
        foreach (var character in allPlayers)
        {
            if (character == null)
                continue;

            float distance = Vector3.Distance(transform.position, character.transform.position);
            if (distance < shortestDistance || shortestDistance < 0)
            {
                nearestTransform = character.transform;
                changed = true;
            }
        }

        return (changed) ? nearestTransform : currentTarget;
    }

    #endregion
}
