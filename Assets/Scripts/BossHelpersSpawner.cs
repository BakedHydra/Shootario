using UnityEngine;

public class BossHelpersSpawner : EnemySpawner
{
    [Header("Boss Helpers Spawner Settings")]
    [SerializeField] string Stage;

    protected override void Start()
    {
        base.Start();
        Boss.SpawnStageEnemies += SpawnBossHelper;
    }

    public void SpawnBossHelper(string stage)
    {
        
        if (stage == Stage)
        {
            base.SpawnEnemy();
        }
    }

}
