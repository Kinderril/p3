using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class ShopSellElement : BaseItemInfo
{
    public Text Desc;
    public void Init(IShopExecute item)
    {
        base.Init(null,false);
        NameLabel.text = "Level:" + item.Parameter;
        mainIcon.sprite = item.icon;
    }
}

