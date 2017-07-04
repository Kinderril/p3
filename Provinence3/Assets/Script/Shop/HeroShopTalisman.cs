using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HeroShopTalisman : HeroShopRandomItem
{
    public override void Execute(int level)
    {
        base.Execute(level,Slot.Talisman);
    }

    public override int CrystalCost
    {
        get { return 1; }
    }

    public override int MoneyCost
    {
        get { return Formuls.ShopPlayerItemCost(Parameter); }
    }
}

