using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public bool stopSpawning = false;
    public float spawnTime = 2f;
    public float spawnDelay = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy1), spawnTime, spawnDelay);
        InvokeRepeating(nameof(SpawnEnemy2), spawnTime, spawnDelay);
    }

    private void SpawnEnemy1()
    {
        GameObject enemy = Instantiate(enemy1Prefab, transform.position, transform.rotation);

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RegisterEnemy();
        }
        if (stopSpawning)
        {
            CancelInvoke(nameof(SpawnEnemy1));
        }
    }

    private void SpawnEnemy2()
    {
        GameObject enemy = Instantiate(enemy2Prefab, transform.position, transform.rotation);

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RegisterEnemy();
        }
        if (stopSpawning)
        {
            CancelInvoke(nameof(SpawnEnemy2));
        }
    }
}
