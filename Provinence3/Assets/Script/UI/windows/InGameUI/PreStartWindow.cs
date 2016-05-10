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
//        FieldLevel.text = level.MissionIndex.ToString();
//        Difficulty.text = level.difficult.ToString();
//        Startpoint.text = level.MissionIndex.ToString();
//        MonsterCountField.text = level.EnemiesKills.ToString();

    }

    public void OnStart()
    {
        gameObject.SetActive(false);
        callback();
    }
}

