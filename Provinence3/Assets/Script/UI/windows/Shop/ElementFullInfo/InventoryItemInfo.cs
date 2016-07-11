using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public abstract class InventoryItemInfo : BaseItemInfo
{
    public Button SellButton;

    protected override void Init(BaseItem item, bool sell = true, bool WithButtons = true)
    {
        base.Init(item, sell, WithButtons);
        SellButton.gameObject.SetActive(WithButtons);
    }

    public void OnSell()
    {
        WindowManager.Instance.ConfirmWindow.Init(() => MainController.Instance.PlayerData.Sell(BaseItem),
            null, "do you wnat to sell it?");
    }

   
}

