using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int numToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        if (!spawnTransform || !enemyPrefab)
            return;

        for (int i = 0; i < numToSpawn; i++)
        {
            Instantiate(enemyPrefab, spawnTransform.position, spawnTransform.rotation);
        }
    }
}
