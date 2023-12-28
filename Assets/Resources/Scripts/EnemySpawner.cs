using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 1f;
    public int amountEnemy = 5;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            StartCoroutine(SpawnEnemiesAtPoint(spawnPoint));
        }

        // Warte, bis alle Spawnpunkte fertig sind
        yield return new WaitForSeconds(amountEnemy * spawnInterval);
    }

    private IEnumerator SpawnEnemiesAtPoint(Transform spawnPoint)
    {
        for (int i = 0; i < amountEnemy; i++)
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemyInstance.transform.SetParent(spawnPoint);
            enemyInstance.AddComponent<EnemyMovement>();

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}