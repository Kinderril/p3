using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class StatElement : MonoBehaviour
{
//    public Text Index;
    public Text MissionIndex;
    public Text GoldCollected;
    public Text MonstersKilled;
    public Text TimeSpendSec;
    public Text BossType;
    public Text QuestsCompleted;
    public Image Back;
    public Color IsGood;
    public Color IsBad;

    public void Init(LevelStat stat)
    {
        MissionIndex.text =  DataBaseController.Instance.MissionNames[stat.MissionIndex];
        GoldCollected.text = stat.GoldCollected.ToString();
        MonstersKilled.text = stat.MonstersKilled.ToString();
        TimeSpendSec.text = stat.TimeSpendSec.ToString();
        BossType.text = stat.BossType.ToString();
        QuestsCompleted.text = stat.QuestsCompleted.ToString();
        if (stat.IsGood)
        {
            Back.color = IsGood;
        }
        else
        {
            Back.color = IsBad;
        }
    }

}

