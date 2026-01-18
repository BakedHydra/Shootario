using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner Settings")]
    [SerializeField] protected GameObject EnemyType;
    protected Vector3 spawnPosition;
    protected Quaternion spawnRotation;
    protected bool isEnemySpawned = false;
    public static event Action OnSpawn;

    protected virtual void Start()
    {
        spawnPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        spawnRotation = transform.rotation;
    }

    public virtual void SpawnEnemy()
    {
        if (!isEnemySpawned)
        {
            Instantiate(EnemyType, spawnPosition, spawnRotation);
            OnSpawn?.Invoke();
            isEnemySpawned = true;
        }
    }
}

