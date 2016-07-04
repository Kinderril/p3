using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public class ShopSellElement : BaseItemInfo
{
    public Text Desc;
    private IShopExecute item;
    public Button BuyButton;
    public void Init(IShopExecute item)
    {
        base.Init(null,false);
        NameLabel.text = "Level:" + item.Parameter;
        mainIcon.sprite = item.icon;
        this.item = item;
        InitCost(item.CrystalCost, item.MoneyCost);

    }
    public void OnBuySimpleChest()
    {
        if (item.CanBuy && EnoughtMoney(item))
        {
            WindowManager.Instance.ConfirmWindow.Init(
                () =>
                {
                    ShopController.Instance.BuyItem(item); 
                }
                , null, "Do you what to but it?");
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, "not enought money");
        }
    }
}

