using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WindowStat : BaseWindow
{
    public Transform Layout;
    public GameObject CoreElement; 
    public override void Init()
    {
        ClearTransform(Layout);
        var coreElement = DataBaseController.GetItem(CoreElement);
        coreElement.transform.SetParent(Layout, false);
        var prefab = DataBaseController.Instance.DataStructs.PrefabsStruct.PrefabStatElement;
        var stats = MainController.Instance.PlayerData.PlayerStats.GetAllStats;
        for (int i = stats.Count - 1; i >= 0; i--)
        {
            var levelStat = stats[i];
            var element = DataBaseController.GetItem<StatElement>(prefab);
            element.Init(levelStat);
            element.transform.SetParent(Layout, false);
        }
        base.Init();
    }
}

