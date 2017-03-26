using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WindowPause : MonoBehaviour
{
    public Text BossProgressField;
    public Transform QuestLayout;
    public Text MonstersKilledField;
    public Text LevelDifficultyField;
    public Text LevelNameField;
    public CurQuestInfo CurQuestInfo;
    private Level level;

    public void Init(Level level)
    {
        gameObject.SetActive(true);
        var map = Map.Instance;
        this.level = level;
        if (map.BossSpawner != null)
        {
            BossProgressField.text = (map.BossSpawner.GetPercent()*100).ToString("0") + "% To Boss Born";
        }
        else
        {
            BossProgressField.text = "";
        }

        Utils.ClearTransform(QuestLayout);
        var curQuest = level.QuestController.CurrentActiveQuest;
        if (curQuest != null)
        {
            var questItem1 = DataBaseController.GetItem<CurQuestInfo>(CurQuestInfo);
            questItem1.Init(curQuest);
            questItem1.transform.SetParent(QuestLayout,false);
        }
        foreach (var finishedQuest in level.QuestController.FinishedQuests)
        {
            var questItem = DataBaseController.GetItem<CurQuestInfo>(CurQuestInfo);
            questItem.Init(finishedQuest);
            questItem.transform.SetParent(QuestLayout, false);
        }
        MonstersKilledField.text = level.LevelStatistics.EnemiesKills + " Monsters killed";
        LevelDifficultyField.text = "Difficulty:"+level.difficult.ToString();
        LevelNameField.text = "Mission:"+ DataBaseController.Instance.MissionNames[level.MissionIndex].ToString();
    }
    
    public void OnResume()
    {
        level.UnPause();
    }

    public void OnSurrender()
    {
        level.UnPause();
        MainController.Instance.level.PreEndLevel(EndlevelType.bad);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

