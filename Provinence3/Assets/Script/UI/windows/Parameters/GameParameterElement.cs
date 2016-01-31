using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class GameParameterElement : MonoBehaviour
{
    public Image Icon;
    private ParamType type;
    public Text field;

    public void Init(ParamType type)
    {
        this.type = type;
        UpgradeData();
        Icon.sprite = DataBaseController.Instance.ParameterIcon(type);
    }

    public void UpgradeData()
    {
        float value = MainController.Instance.PlayerData.CalcParameter(type);
        field.text = value.ToString("0");
    }
}

