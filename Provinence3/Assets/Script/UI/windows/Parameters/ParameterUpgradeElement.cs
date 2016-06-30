using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ParameterUpgradeElement : MonoBehaviour
{
    public Button UpgradeButton;
    public Text CurrentValue;
    private MainParam type;
//    public Image Icon;
    public Text paramInfo;
    public Text nameField;

    public MainParam TypeParam
    {
        get { return type; }
    }

    public void Init(MainParam type)
    {
        this.type = type;
        UpgradeData();
        nameField.text = Localizer.MainParameterName(type);
        paramInfo.text = Localizer.MainParameterInfo(type);
        //        Icon.sprite = DataBaseController.Instance.MainParameterIcon(type);
    }

    public void UpgradeData()
    {
        var val = MainController.Instance.PlayerData.MainParameters[type];
        CurrentValue.text = val.ToString();
        //ebug.Log("UPG: " + val);
        //nextLevelCost.text = DataBaseController.Instance.DataStructs.costParameterByLvl[val].ToString("0");
        UpgradeButton.interactable = MainController.Instance.PlayerData.CanUpgradeParameter();
    }

    public void OnUpgradeClick()
    {
        MainController.Instance.PlayerData.UpgdareParameter(type);
    }
}

