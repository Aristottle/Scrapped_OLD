using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] EnemyProfile enemyProfile;

    private Transform currentTarget;
    private BehaviorState currentBehavior = BehaviorState.Idle;

    private NavMeshAgent navMeshAgent;
    private CharacterHealth characterHealth;

    private void Awake() 
    {
        if (!enemyProfile)
        {
            Debug.Log("ERR: behaviorProfile not set on AI");
            Destroy(gameObject);
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        characterHealth = GetComponent<CharacterHealth>();

        // init the character health
        characterHealth.InitializeHealth(enemyProfile);
    }

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
        GameObject nearestPlayer = GetNearestPlayer();
        if (nearestPlayer != null)
        {
            currentTarget = nearestPlayer.transform;
            if (!CheckDesiredRange(enemyProfile.desiredCombatRange))
            {
                ChangeState(BehaviorState.Chasing);
            }
        }
        else
        {
            Debug.Log("ERR: Nearest player == null");
        }
    }
    
    void _ChaseUpdate()
    {
        if (currentTarget)
        {
            // Debug.Log(currentTarget.position);
            if (!CheckDesiredRange(enemyProfile.desiredCombatRange))
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(currentTarget.position);
            }
            else navMeshAgent.isStopped = true;
        }
        else ChangeState(BehaviorState.Idle);
    }

    void _AttackUpdate()
    {

    }

    void _EntryUpdate()
    {

    }

    #endregion

    #region Private Helpers

    GameObject GetNearestPlayer()
    {
        List<GameObject> allPlayers = PlayerManager.GetPlayerCharacters();

        GameObject nearest = null;

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
                nearest = character;
            }
        }

        return nearest;
    }

    private bool CheckDesiredRange(float range)
    {
        return Vector3.Distance(transform.position, currentTarget.position) <= range;
    }

    private void ChangeState(BehaviorState newState)
    {
        currentBehavior = newState;
        onBehaviorStateChanged?.Invoke(newState);
        Debug.Log($"State changed to {newState}");
    }

    #endregion
}
