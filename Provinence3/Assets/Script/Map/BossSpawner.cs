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
    public event Action<int, int> OnBossGetEnergy;
    private bool isBossSpawned = false;
//    private float PRECENT_TO_KILL = 0.13f;
    private int bonusesGet = 0;

    public BossSpawner(int count, Action<int> SpawnBoss,float precentToKill )
    {
        enemiesKilled = 0;
        OnSpawnBoss = SpawnBoss;
        ToSpawnBossOnStart =(int)(count * precentToKill );
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
            if (OnBossGetEnergy != null)
            {
                OnBossGetEnergy(enemiesKilled,ToSpawnBossOnStart);
            }
            if (enemiesKilled >= ToSpawnBossOnStart)
            {
                isBossSpawned = true;
                OnSpawnBoss(bonusesGet);
            }
        }
    }

    public float GetPercent()
    {
        return (float)enemiesKilled/(float)ToSpawnBossOnStart;
    }

    public void GetBonus()
    {
        bonusesGet++;
    }
}

