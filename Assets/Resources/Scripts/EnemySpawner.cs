using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemyInstance.transform.SetParent(spawnPoint);
            enemyInstance.AddComponent<EnemyMovement>();
        }
    }
}
