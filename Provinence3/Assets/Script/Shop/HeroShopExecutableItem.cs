using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HeroShopExecutableItem : IShopExecute
{
    public override void Execute(int parameter)
    {
            MainController.Instance.PlayerData.AddItem(CreatExecutableItem(parameter));
    }

    public static ExecutableItem CreatExecutableItem(int lvl)
    {
        var t = ShopController.AllExecutables.RandomElement();
        return new ExecutableItem(t);
    }
}

