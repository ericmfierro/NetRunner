using UnityEngine;
using System.Collections;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] Transform[] spawnPoints;

    [Header("Spawn Settings")]
    [SerializeField] float spawnInterval = 5f;
    [SerializeField] int maxEnemies = 20;

    int currentEnemies = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Spawner missing prefab or spawn points");
            return;
        }

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawn.position, Quaternion.identity);
        currentEnemies++;
    }
}
