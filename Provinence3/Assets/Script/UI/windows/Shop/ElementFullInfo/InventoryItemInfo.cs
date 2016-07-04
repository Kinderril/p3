using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public abstract class InventoryItemInfo : BaseItemInfo
{
    public Button SellButton;
    
    public void OnSell()
    {
        WindowManager.Instance.ConfirmWindow.Init(() => MainController.Instance.PlayerData.Sell(BaseItem),
            null, "do you wnat to sell it?");
    }

   
}

