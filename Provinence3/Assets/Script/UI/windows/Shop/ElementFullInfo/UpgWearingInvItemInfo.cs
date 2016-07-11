using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;


public abstract class UpgWearingInvItemInfo : WearingInventoryItemInfo
{
    public Button UpgradeButton;
    public Text EnchantField;

    protected override void Init(BaseItem item, bool sell = true,bool WithButtons = true)
    {
        var canBeupgraded = MainController.Instance.PlayerData.CanBeUpgraded(item) != null;
        UpgradeButton.interactable = canBeupgraded;
        base.Init(item, sell, WithButtons);
        UpgradeButton.gameObject.SetActive(UpgradeButton);

    }

    public void SetEnchant(int count,bool haveEnchant)
    {
        EnchantField.gameObject.SetActive(haveEnchant);
        EnchantField.text = "+" + count + " Power";
    }

    public void OnUpgradeClick()
    {
        var playerItem = BaseItem as PlayerItem;
        var shop = WindowManager.Instance.CurrentWindow as WindowShop;
        if (shop != null)
        {
            shop.UpgradeWindow.Init(playerItem, OnItemEnchanted);
        }
    }
    private void OnItemEnchanted(PlayerItem obj)
    {
        Refresh();
    }
}

