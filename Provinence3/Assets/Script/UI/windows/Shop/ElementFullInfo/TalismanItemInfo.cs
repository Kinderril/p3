using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class TalismanItemInfo : BaseItemInfo
{
    public Text enchantField;
    public Text descField;
    public Text powerField;
    public void Init(TalismanItem talismanItem)
    {
        base.Init(talismanItem);
        mainIcon.sprite = DataBaseController.Instance.TalismanIcon(talismanItem.TalismanType);
        powerField.text = "power:" + talismanItem.points;
        descField.text = "cost:"  + talismanItem.costShoot;
        NameLabel.text = talismanItem.name;
    }
}
