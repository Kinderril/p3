using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelStat
{
    private const char DELEM = '-';

    public int Index;
    public int MissionIndex;
    public int GoldCollected;
    public int MonstersKilled;
    public int TimeSpendSec;
    public string BossType;
    public int QuestsCompleted;
    public bool IsGood;

    public string Save()
    {
        var ss = Index.ToString() + DELEM +//0
                 MissionIndex.ToString() + DELEM +//1
                 GoldCollected.ToString() + DELEM +//2
                 MonstersKilled.ToString() + DELEM +//3
                 TimeSpendSec.ToString() + DELEM +//4
                 BossType.ToString() + DELEM +//5
                 QuestsCompleted.ToString() + DELEM +//6
                 IsGood.ToString() + DELEM;//7
        return ss;
    }
    public static LevelStat Load(string data)
    {
        var ss = data.Split(DELEM);
        var Index = Convert.ToInt32(ss[0]);
        var MissionIndex = Convert.ToInt32(ss[1]);
        var GoldCollected = Convert.ToInt32(ss[2]);
        var MonstersKilled = Convert.ToInt32(ss[3]);
        var TimeSpendSec = Convert.ToInt32(ss[4]);
        var BossType = (ss[5]);
        var QuestsCompleted = Convert.ToInt32(ss[6]);
        var IsGood = Convert.ToBoolean(ss[7]);
        LevelStat levelStat = new LevelStat()
        {
            BossType = BossType,
            GoldCollected = GoldCollected,
            Index = Index,
            MissionIndex = MissionIndex,
            MonstersKilled = MonstersKilled,
            QuestsCompleted = QuestsCompleted,
            TimeSpendSec = TimeSpendSec,
            IsGood = IsGood
        };
        return levelStat;
    }
}

public class PlayerStats
{
    private const string PLAYER_PREF = "PlayerStats";
    private const char DELEM_BG = '=';
    private List<LevelStat> stats;

    public List<LevelStat>  GetAllStats
    {
        get { return stats; }
    }

    public void Save()
    {
        string data = "";
        foreach (var levelStat in stats)
        {
            data += levelStat.Save() + DELEM_BG;
        }
        PlayerPrefs.SetString(PLAYER_PREF, data);

    }

    public void Load()
    {
        stats = new List<LevelStat>();
        var data = PlayerPrefs.GetString(PLAYER_PREF);
        var ss = data.Split(DELEM_BG);
        foreach (var s in ss)
        {
            if (s.Length > 5)
            {
                stats.Add(LevelStat.Load(s));
            }
        }
        Debug.Log("Stats load: " + stats.Count);
    }

    public void AddLevel(Level level,LevelStatistics statistics)
    {
        LevelStat levelStat = new LevelStat();
        levelStat.Index = stats.Count;
        levelStat.BossType = statistics.BossType;
        levelStat.GoldCollected = level.GetAllCollectedMoney()[ItemId.money];
        levelStat.MissionIndex = level.MissionIndex;
        levelStat.QuestsCompleted = level.QuestController.CompletedQuests();
        levelStat.TimeSpendSec = (int)statistics.GetElapsetime().TotalSeconds;
        levelStat.MonstersKilled = statistics.EnemiesKills;
        levelStat.IsGood = level.IsGoodEnd == EndlevelType.good;
        stats.Add(levelStat);
        Save();
    }
}

