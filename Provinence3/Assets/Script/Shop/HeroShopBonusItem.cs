using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HeroShopBonusItem : IShopExecute
{
    public override void Execute(int lvl)
    {
        var item = CreatBonusItem(lvl);
        MainController.Instance.PlayerData.AddItem(item);
        base.Execute(lvl);
    }

    public static BonusItem CreatBonusItem(int lvl)
    {
        var bonus = ShopController.RandomBonus();
        BonusItem b = new BonusItem(bonus, lvl * 2, 3);
        return b;
    }
}

