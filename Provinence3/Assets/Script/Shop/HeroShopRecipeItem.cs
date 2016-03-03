using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public  class HeroShopRecipeItem : IShopExecute
{
    public HeroShopRecipeItem(int lvl) : base(lvl)
    {
//        icon = ;
        name = "Recipe Chest";
    }

    public override void Execute(int parameter)
    {
        MainController.Instance.PlayerData.AddItem(CreatRandomRecipeItem(parameter));
        base.Execute(parameter);
    }

    public static RecipeItem CreatRandomRecipeItem(int lvl)
    {
        return new RecipeItem(lvl, ShopController.RandomSlot());
    }
}

