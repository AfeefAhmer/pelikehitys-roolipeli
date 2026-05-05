using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnedEnemy;

    private int totalSpawned = 0;
    private int totalDead = 0;

    private int maxTotal = 20;
    private int batchSize = 5;

    private bool playerInRange = false;
    private bool isSpawning = false;
    public event UnityAction SpawnerEnd;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!isSpawning)
            {
                StartCoroutine(SpawnEnemies());
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        isSpawning = true;

        int spawnedInBatch = 0;

        while (spawnedInBatch < batchSize && totalSpawned < maxTotal)
        {
            SpawnEnemy();

            spawnedInBatch++;
            totalSpawned++;

            yield return new WaitForSeconds(1f);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        GameObject enemyObj = Instantiate(spawnedEnemy, transform.position, Quaternion.identity);

        IDamageable dmg = enemyObj.GetComponent<IDamageable>();

        if (dmg != null)
        {
            dmg.OnDeath += OnEnemyDead; 
        }
    }

    void OnEnemyDead()
    {
        totalDead++;

        // Voidaanko spawnata lisää?
        if (playerInRange && totalSpawned < maxTotal)
        {
            if (!isSpawning)
            {
                StartCoroutine(SpawnEnemies());
            }
        }

        // Kaikki spawnattu JA tapettu → tuhoa spawner
        if (totalDead >= maxTotal)
        {
            SpawnerEnd.Invoke();
            Destroy(gameObject);
        }
    }
}