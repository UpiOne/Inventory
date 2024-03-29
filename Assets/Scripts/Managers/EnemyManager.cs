using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnRadius = 5f; 

    private void Start()
    {
        SpawnEnemies(3); 
    }

    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemyPrefab = enemyList[Random.Range(0, enemyList.Count)];

            Vector2 randomSpawnPos = spawnPoint.position + Random.insideUnitSphere * spawnRadius;

            GameObject enemy = Instantiate(enemyPrefab, randomSpawnPos, Quaternion.identity);

            enemy.transform.parent = transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
