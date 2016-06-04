using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class BonusItemInfo : BaseItemInfo
{
    public Text descField;
    public Text remainTimesField;
    public void Init(BonusItem bonusItem)
    {
        base.Init(bonusItem);

        //        var element = DataBaseController.GetItem<ParameterElement>(Prefab);
        //        element.Init(ParamType.PPower, bonusItem.power);
        //        element.transform.SetParent(layout);
        NameLabel.text = "name (";
        remainTimesField.text = "Remain:" + bonusItem.remainUsetime;
    }
}
