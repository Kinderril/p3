using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanHeal : Talisman
{
    public TalismanHeal(TalismanItem sourseItem, int countTalismans) 
        : base(sourseItem, countTalismans)
    {

    }
    public override void Use()
    {
        MainController.Instance.level.MainHero.GetHeal(currentEnergy);
        base.Use();
    }
}

