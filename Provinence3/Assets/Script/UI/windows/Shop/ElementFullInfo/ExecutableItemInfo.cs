using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ExecutableItemInfo : InventoryItemInfo
{
    public Text descField;
    public void Init(ExecutableItem executableItem,bool WithButtons)
    {
        base.Init(executableItem,true, WithButtons);
        switch (executableItem.ExecutableType)
        {
            case ExecutableType.craft:
                var craft = executableItem as ExecCraftItem;
                if (craft != null)
                {
                    NameLabel.text = craft.Name;
                    mainIcon.sprite = craft.IconSprite;
                    descField.text = "Use this for craft weapons and armor";
                }
                break;
            case ExecutableType.enchant:
                var enchant = executableItem as ExecEnchantItem;
                if (enchant != null)
                {
                    NameLabel.text = enchant.Name;
                    mainIcon.sprite = enchant.IconSprite;
                    string desc = "";
                    switch (enchant.ItemType)
                    {
                        case EnchantType.weaponUpdate:
                            desc = "Use this to upgrade your weapon power";
                            break;
                        case EnchantType.powerUpdate:
                            desc = "Use for enchant items to upgrade power";
                            break;
                        case EnchantType.armorUpdate:
                            desc = "Use this to upgrade your armor";
                            break;
                    }
                    descField.text = desc;
                }
                break;
            case ExecutableType.catalys:
                var catalys = executableItem as ExecCatalysItem;
                if (catalys != null)
                {
                    NameLabel.text = catalys.Name;
                    mainIcon.sprite = catalys.IconSprite;
                    descField.text = "Use for craft weapon to add special abilities";
                }
                break;
        }
    }

    public override void OnSell()
    {
        ExecutableItem executableItem = BaseItem as ExecutableItem;
        WindowManager.Instance.ConfirmWindowWithCount.Init(OnSellOk,null, "do you wnat to sell it?", executableItem.count);
    }

    private void OnSellOk(int count)
    {
        MainController.Instance.PlayerData.Sell(BaseItem, count);
    }
}
