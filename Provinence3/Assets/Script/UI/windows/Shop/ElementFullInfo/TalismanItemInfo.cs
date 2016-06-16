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

        var item = Talisman.Creat(talismanItem, 0, null);

        mainIcon.sprite = DataBaseController.Instance.TalismanIcon(talismanItem.TalismanType);
        powerField.text = item.PowerInfo();
        descField.text = "cost:"  + talismanItem.costShoot;
        NameLabel.text = talismanItem.TalismanType.ToString();
    }
}
