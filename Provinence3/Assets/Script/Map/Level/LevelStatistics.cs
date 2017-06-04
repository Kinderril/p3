using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LevelStatistics
{
    private float startTime;
    private float ElapsedTime;
    public int EnemiesKills = 0;
    public string BossName;

    public LevelStatistics()
    {
        startTime = Time.time;
    }

    public string BossType = "no type";

    public void End()
    {
        ElapsedTime = Time.time - startTime;
    }

    public TimeSpan GetElapsetime()
    {
        return TimeSpan.FromSeconds(ElapsedTime);
    }

    public bool GetReward(int questCompleted, Dictionary<CraftItemType, int> collectedCrafts,bool bossKilled)
    {
        var sumCrafts = collectedCrafts.Values.Sum();
        int total = EnemiesKills + questCompleted*17 + (int)(sumCrafts*2.3f);
        total += bossKilled ? 10 : 0;
        return total > 100;
    } 
}

