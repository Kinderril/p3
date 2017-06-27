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
    public Transform BonusLayout;
    public BonusItemView PrefaBonusItemView;
    public void Init(Level level,Action callback)
    {
        Utils.ClearTransform(BonusLayout);
        gameObject.SetActive(true);
        this.callback = callback;
        var bonuses = level.MainHero.Bonuses;
        BonusLayout.gameObject.SetActive(bonuses.Any());
        foreach (var bonusItem in bonuses)
        {
            var item = DataBaseController.GetItem<BonusItemView>(PrefaBonusItemView);
            item.transform.SetParent(BonusLayout, false);
            item.Init(bonusItem);
        }
        if (DataBaseController.Instance.MissionNames.ContainsKey(level.MissionIndex))
        {

            FieldLevel.text = DataBaseController.Instance.MissionNames[level.MissionIndex];
            var names = DataBaseController.Instance.RespawnPositionsNames[level.MissionIndex];
            Difficulty.text = "Difficulty:" + level.difficult.ToString();
            Startpoint.text = names[level.IndexBornPoint];
        }

    }

    public void OnStart()
    {
        gameObject.SetActive(false);
        callback();
    }
}

