using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ExecutableItemInfo : BaseItemInfo
{
    public Text descField;
    public void Init(ExecutableItem executableItem)
    {
        base.Init(executableItem);
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
                    descField.text = "Use for enchant items to upgrade power";
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
}
