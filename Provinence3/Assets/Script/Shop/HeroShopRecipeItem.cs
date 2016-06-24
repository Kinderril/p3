﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public  class HeroShopRecipeItem : IShopExecute
{
    public override void Init(int lvl)
    {
        base.Init(lvl);
        name = "Recipe Chest";
    }

    public override void Execute(int parameter)
    {
        MainController.Instance.PlayerData.AddItem(CreatRandomRecipeItem(parameter));
        base.Execute(parameter);
    }
    public override int MoneyCost
    {
        get { return Formuls.RecepiCost(Parameter); }
    }

    public static RecipeItem CreatRandomRecipeItem(int lvl)
    {
        return new RecipeItem(lvl, ShopController.RandomSlot());
    }
}

