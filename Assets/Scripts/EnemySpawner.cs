using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner Settings")]
    [SerializeField] private GameObject EnemyType;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Start()
    {
        spawnPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        spawnRotation = transform.rotation;
    }

    public void SpawnEnemy()
    {
        Instantiate(EnemyType, spawnPosition, spawnRotation);
    }
}

