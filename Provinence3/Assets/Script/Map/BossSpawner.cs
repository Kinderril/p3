using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BossSpawner
{
//    private int EnemiesOnStart;
    private int enemiesKilled;
    private int ToSpawnBossOnStart;
    private Action<int> OnSpawnBoss;
    private bool isBossSpawned = false;
    private float PRECENT_TO_KILL = 0.16f;
    private int bonusesGet = 0;

    public BossSpawner(int count, Action<int> SpawnBoss)
    {
        enemiesKilled = 0;
        OnSpawnBoss = SpawnBoss;
        ToSpawnBossOnStart =(int)(count * PRECENT_TO_KILL );
#if UNITY_EDITOR
        if (DebugController.Instance.LESS_COUNT_BOSS_COME)
        {
            ToSpawnBossOnStart = 1;
        }
#endif
    }

    public void EnemieDead()
    {
        if (!isBossSpawned)
        {
            enemiesKilled++;
            Debug.Log("ToSpawnBossOnStart " + ToSpawnBossOnStart + " > " + enemiesKilled);
            if (enemiesKilled > ToSpawnBossOnStart)
            {
                isBossSpawned = true;
                OnSpawnBoss(bonusesGet);
            }
        }
    }

    public void GetBonus()
    {
        bonusesGet++;
    }
}

