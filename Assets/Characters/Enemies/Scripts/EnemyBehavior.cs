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
    }

    #region States

    void _IdleUpdate()
    {
        
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
        return null;
    }

    #endregion
}
