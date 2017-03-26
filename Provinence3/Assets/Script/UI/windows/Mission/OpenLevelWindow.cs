using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class OpenLevelWindow : MonoBehaviour
{
    public Text CrystalsCost;
//    public Text LevelField;
    private int index;
    private Action<int> OnOpenLevelCallback;
    private Action OnWindowClose;

    public void Init(int index,Action<int> OnOpenLevelCallback,Action OnWindowClose)
    {
        this.OnOpenLevelCallback = OnOpenLevelCallback;
        this.OnWindowClose = OnWindowClose;
        this.index = index;
        gameObject.SetActive(true);
        var cost = DataBaseController.Instance.LevelsCost[index];
        CrystalsCost.text = cost.Crystals.ToString("0");
//        LevelField.text = cost.PlayerLevel.ToString("0");
    }

    public void OnOpenClick()
    {
        MainController.Instance.PlayerData.OpenLevels.OpenLevel(index,true);
        OnOpenLevelCallback(index);
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnWindowClose();
    }
}

