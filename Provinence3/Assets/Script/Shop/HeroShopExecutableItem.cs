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

    public static ExecEnchantItem CreatExecutableItem(int lvl)
    {
        var t = ShopController.AllEnchantes.RandomElement();
        
        return new ExecEnchantItem(t,1);
    }

    public HeroShopExecutableItem(int lvl) 
        : base(lvl)
    {
        name = "Scroll";
    }
}

