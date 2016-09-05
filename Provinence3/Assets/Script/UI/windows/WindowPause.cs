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
        BossProgressField.text = map.BossSpawner.GetPercent() + "% To Boss Born";
        QuestField.text = level.QuestController.CurrentQuestInfo();
        MonstersKilledField.text = level.EnemiesKills + " Monsters killed";
        LevelDifficultyField.text = "Difficulty:"+level.difficult.ToString();
        LevelNameField.text = "Mission index:"+level.MissionIndex.ToString();
    }
    
    void OnResume()
    {
        level.UnPause();
    }

    void OnSurrender()
    {
        level.UnPause();
        MainController.Instance.EndLevel(EndlevelType.bad);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

