using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public  class HeroShopCheat : IShopExecute
{
    public override void Init(int lvl)
    {
        base.Init(lvl);
        name = "Cheat Chest";
    }
    public override int CrystalCost
    {
        get { return 0; }
    }

    public override void Execute(int parameter)
    {
        MainController.Instance.PlayerData.AddItem(new ExecCatalysItem(CatalysItemType.black, 2));
        MainController.Instance.PlayerData.AddItem(new ExecCatalysItem(CatalysItemType.white, 2));
        MainController.Instance.PlayerData.AddItem(new ExecCatalysItem(CatalysItemType.green, 2));
        MainController.Instance.PlayerData.AddItem(new ExecCatalysItem(CatalysItemType.red, 2));
        MainController.Instance.PlayerData.AddItem(new ExecCatalysItem(CatalysItemType.blue, 2));

        foreach (CraftItemType v in Enum.GetValues(typeof(CraftItemType)))
        {
            var a = new ExecCraftItem(v, 50);
            MainController.Instance.PlayerData.AddItem(a);
        }

        base.Execute(parameter);
    }
    public override int MoneyCost
    {
        get { return 1; }
    }
}

