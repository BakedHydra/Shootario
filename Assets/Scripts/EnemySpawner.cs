using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner Settings")]
    [SerializeField] protected GameObject EnemyType;
    protected Vector3 spawnPosition;
    protected Quaternion spawnRotation;
    public static event Action OnSpawn;

    protected virtual void Start()
    {
        spawnPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        spawnRotation = transform.rotation;
    }

    public virtual void SpawnEnemy()
    {
        Instantiate(EnemyType, spawnPosition, spawnRotation);
        OnSpawn?.Invoke();
    }
}

