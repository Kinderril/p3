using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HeroShopExecutableItem : IShopExecute
{
    public override void Execute(int parameter)
    {
        MainController.Instance.PlayerData.AddItem(CreatExecutableItem(parameter));
        base.Execute(parameter);
    }

    public override int MoneyCost
    {
        get { return Formuls.ExecutableCost(Parameter); }
    }
    public override int CrystalCost
    {
        get { return 1; }
    }
    public static ExecEnchantItem CreatExecutableItem(int lvl)
    {
        var t = ShopController.AllEnchantes.RandomElement();
        
        return new ExecEnchantItem(t,1);
    }

    public override void Init(int lvl)
    {
        base.Init(lvl);
        name = "Scroll";
    }
}

