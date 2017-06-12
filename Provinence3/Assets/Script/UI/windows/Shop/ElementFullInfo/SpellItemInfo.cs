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
        powerField.text = spellItem.SpellData.ValueGold.ToString() + "TODO";
        descField.text = spellItem.SpellData.Desc();
        NameLabel.text = spellItem.SpellData.Name;
        
        SetEnchant(spellItem.enchant, spellItem.enchant > 0);
    }
}
