using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BossSpawner
{
    private int EnemiesOnStart;
    private int ToSpawnBossOnStart;
    private Action OnSpawnBoss;
    private bool isBossSpawned = false;
    private const float PRECENT_TO_KILL = 0.05f;

    public BossSpawner(int count, Action SpawnBoss)
    {
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
                OnSpawnBoss();
            }
        }
    }
}

