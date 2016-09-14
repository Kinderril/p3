using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class BonusItemInfo : WearingInventoryItemInfo
{
    public Text descField;
    public Text remainTimesField;
    public void Init(BonusItem bonusItem,bool WithButtons)
    {
        base.Init(bonusItem,true, WithButtons);

        string nameBonus = "";
        string descBonus = "";
        var percent = (bonusItem.power*100).ToString("00");
        switch (bonusItem.Bonustype)
        {
            case Bonustype.damage:
                nameBonus = "Additional Damage";
                descBonus = " Give " + percent + "% additional damage at next round.\n Use 1 charge per round";
                break;
            case Bonustype.money:
                nameBonus = "Bonus Money";
                descBonus = " Find more money for " + percent + "% for all coin you can find.\n Use 1 charge per round";
                break;
            case Bonustype.cryslats:
                nameBonus = "Bonus Crystals";
                descBonus = " Increase chance to find for " + percent + "%.\n Use 1 charge per round";
                break;
            case Bonustype.defence:
                nameBonus = "Additional Defence";
                descBonus = " Give " + percent + "% additional defence at next round.\n Use 1 charge per round";
                break;
            case Bonustype.energy:
                nameBonus = "Bonus Energy";
                descBonus = " Increase energy incoming for " + percent + "%.\n Use 1 charge per round";
                break;
            case Bonustype.maxHp:
                nameBonus = "Power health";
                descBonus = " Increase maximum healths points for " + percent + "%.\n Use 1 charge per round";
                break;
            case Bonustype.speed:
                nameBonus = "Additional Speed";
                descBonus = " Give " + percent + "% additional speed at next round.\n Use 1 charge per round";
                break;
        }

        mainIcon.sprite = bonusItem.IconSprite;
        NameLabel.text = nameBonus;
        descField.text = descBonus;
        remainTimesField.text = "Remain:" + bonusItem.remainUsetime;
    }
}
