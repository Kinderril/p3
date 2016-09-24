using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class WindowStart : BaseWindow
{
    public GameObject StartBoost;
    public override void Init()
    {
        base.Init();
        WindowManager.Instance.MainBack.gameObject.SetActive(true);
        StartBoost.SetActive(MainController.Instance.PlayerData.CheckIfFirstLevel());
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnClearAllClick()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OnGetStartBoost()
    {
        MainController.Instance.PlayerData.AddStartEquipment();
        StartBoost.SetActive(false);
        OnToShop();
    }
}

