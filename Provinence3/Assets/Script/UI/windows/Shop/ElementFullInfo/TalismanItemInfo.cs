using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class TalismanItemInfo : UpgWearingInvItemInfo
{
    public Text descField;
    public Text powerField;
    public void Init(TalismanItem talismanItem,bool WithButtons)
    {
        base.Init(talismanItem,true, WithButtons);

        var item = Talisman.Creat(talismanItem, 0, null);

        mainIcon.sprite = DataBaseController.Instance.TalismanIcon(talismanItem.TalismanType);
        powerField.text = item.PowerInfo();
        descField.text = "Cost:"  + talismanItem.costShoot.ToString("0") + "\n Charges:" + talismanItem.MaxCharges;
        NameLabel.text = TalismanName(talismanItem.TalismanType);
        
        SetEnchant(talismanItem.enchant, talismanItem.enchant > 0);
    }

    public static string TalismanName(TalismanType type)
    {
        string name = "no name";
        switch (type)
        {
            case TalismanType.firewave:
                name = "Firewave";
                break;
            case TalismanType.massPush:
                name = "Round punch";
                break;
            case TalismanType.massFreez:
                name = "Freez field";
                break;
            case TalismanType.heal:
                name = "Heal";
                break;
            case TalismanType.doubleDamage:
                name = "Overcharge";
                break;
            case TalismanType.megaArmor:
                name = "Ice armor";
                break;
            case TalismanType.chain:
                name = "Chain strike";
                break;
            case TalismanType.trapDamage:
                name = "Shooting totem";
                break;
            case TalismanType.trapAOE:
                name = "Explosive totem";
                break;
            case TalismanType.trapFreez:
                name = "Freezing totem";
                break;
            case TalismanType.bloodDamage:
                name = "Rapture strike";
                break;
            case TalismanType.energyVamp:
                name = "Soul take";
                break;
            case TalismanType.splitter:
                name = "Equalizer";
                break;
        }
        return name;
    }
}
