using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public abstract class WearingInventoryItemInfo : InventoryItemInfo
{
    public Button EquipButton;
    public Button UnEquipButton;

    protected override void Init(BaseItem item, bool sell = true, bool WithButtons = true)
    {
        base.Init(item, sell, WithButtons);
        EquipButton.gameObject.SetActive(WithButtons);
        UnEquipButton.gameObject.SetActive(WithButtons);
    }

    public void OnUnequipItem()
    {
        MainController.Instance.PlayerData.EquipItem(BaseItem, false);
    }
    public void OnEquip()
    {
        MainController.Instance.PlayerData.EquipItem(BaseItem);
    }
}

