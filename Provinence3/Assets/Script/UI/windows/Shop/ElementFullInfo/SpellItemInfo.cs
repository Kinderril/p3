using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class SpellItemInfo : UpgWearingInvItemInfo
{
    public Text descField;
    public Text powerField;
    public void Init(SpellItem spellItem,bool WithButtons)
    {
        base.Init(spellItem,true, WithButtons);

//        var item = Talisman.Execute(talismanItem, 0, null);
//        slo
        mainIcon.sprite = spellItem.IconSprite;
        powerField.text ="Charges:" + spellItem.SpellData.Charges.ToString("0") + "\n" +
                          "Cost:" + spellItem.SpellData.Cost.ToString("0") + "\n" +
                          "Level:" + spellItem.SpellData.Level.ToString("0");
        descField.text = spellItem.SpellData.Desc();
        NameLabel.text = spellItem.SpellData.Name;
        
        SetEnchant(spellItem.enchant, spellItem.enchant > 0);
    }
}
