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
        switch (bonusItem.Bonustype)
        {
            case Bonustype.damage:
                nameBonus = "Bonus Damage";
                descBonus = " Give additional damage at next round, for all time.\n Use 1 charge per round";
                break;
            case Bonustype.money:
                nameBonus = "Additional Money";
                descBonus = " Add some money for all coin you can find , for all time.\n Use 1 charge per round";
                break;
        }


        NameLabel.text = nameBonus;
        descField.text = descBonus;
        remainTimesField.text = "Charges remain:" + bonusItem.remainUsetime;
    }
}
