using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PreStartWindow : MonoBehaviour
{
    public Button StartButton;
    private Action callback;
    public Text FieldLevel;
    public Text Difficulty;
    public Text Startpoint;
    public Text MonsterCountField;
    public Text ActiveBonuses;
    public void Init(Level level,Action callback)
    {
        gameObject.SetActive(true);
        this.callback = callback;
        FieldLevel.text = "Level:"+level.MissionIndex.ToString();
        var names = DataBaseController.Instance.RespawnPositionsNames[level.MissionIndex];
        Difficulty.text = "Difficulty:"+level.difficult.ToString();
        Startpoint.text = names[level.IndexBornPoint];
        MonsterCountField.text = "Monsters:"+Map.Instance.enemies.Count.ToString();
//        if (level.MainHero.CurrenthBonus)

    }

    public void OnStart()
    {
        gameObject.SetActive(false);
        callback();
    }
}

