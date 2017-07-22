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
    private int cost;

    public void Init(int index,Action<int> OnOpenLevelCallback,Action OnWindowClose)
    {
        this.OnOpenLevelCallback = OnOpenLevelCallback;
        this.OnWindowClose = OnWindowClose;
        this.index = index;
        gameObject.SetActive(true);
        cost = DataBaseController.Instance.LevelsCost[index].Crystals;
        CrystalsCost.text =  "Crystals cost to open level:" + cost.ToString("0");
//        LevelField.text = cost.PlayerLevel.ToString("0");
    }

    public void OnOpenClick()
    {
        if (MainController.Instance.PlayerData.CanPay(ItemId.crystal, cost))
        {
            MainController.Instance.PlayerData.OpenLevels.OpenLevel(index, true);
            OnOpenLevelCallback(index);
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null,"You don't have enought crystals");
        }
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnWindowClose();
    }
}

