using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WindowPause : MonoBehaviour
{
    public Text BossProgressField;
    public Text QuestField;
    public Text MonstersKilledField;
    public Text LevelDifficultyField;
    public Text LevelNameField;
    private Level level;

    public void Init(Level level)
    {
        gameObject.SetActive(true);
        var map = Map.Instance;
        this.level = level;
        if (map.BossSpawner != null)
        {
            BossProgressField.text = map.BossSpawner.GetPercent().ToString("0.00") + "% To Boss Born";
        }
        else
        {
            BossProgressField.text = "";
        }
        QuestField.text = level.QuestController.CurrentQuestInfo();
        MonstersKilledField.text = level.LevelStatistics.EnemiesKills + " Monsters killed";
        LevelDifficultyField.text = "Difficulty:"+level.difficult.ToString();
        LevelNameField.text = "Mission index:"+level.MissionIndex.ToString();
    }
    
    public void OnResume()
    {
        level.UnPause();
    }

    public void OnSurrender()
    {
        level.UnPause();
        MainController.Instance.EndLevel(EndlevelType.bad);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

