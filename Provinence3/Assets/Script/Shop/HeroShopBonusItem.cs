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

    public override int MoneyCost
    {
        get { return Formuls.BonusCost(Parameter); }
    }

    public static BonusItem CreatBonusItem(int lvl)
    {
        var bonus = ShopController.RandomBonus();
        BonusItem b = new BonusItem(bonus, Formuls.GetBonusPointsByLvl(lvl), BonusItem.BONUS_USE_TIME);
        return b;
    }
    
    public override void Init(int lvl)
    {
        base.Init(lvl);
        name = "Bonus";
    }
}

