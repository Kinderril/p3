using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BossSpawner
{
    private int EnemiesOnStart;
    private int ToSpawnBossOnStart;
    private Action<int> OnSpawnBoss;
    private bool isBossSpawned = false;
    private float PRECENT_TO_KILL = 0.32f;
    private int bonusesGet = 0;

    public BossSpawner(int count, Action<int> SpawnBoss)
    {
#if UNITY_EDITOR
        if (DebugController.Instance.LESS_COUNT_BOSS_COME)
        {
            PRECENT_TO_KILL = 0.01f;
        }
#endif

        OnSpawnBoss = SpawnBoss;
        this.EnemiesOnStart = count;
        ToSpawnBossOnStart =(int)( EnemiesOnStart*(1f-PRECENT_TO_KILL) );
    }

    public void EnemieDead()
    {
        if (!isBossSpawned)
        {
            EnemiesOnStart--;
            Debug.Log("ToSpawnBossOnStart " + ToSpawnBossOnStart + " > " + EnemiesOnStart);
            if (ToSpawnBossOnStart > EnemiesOnStart)
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

