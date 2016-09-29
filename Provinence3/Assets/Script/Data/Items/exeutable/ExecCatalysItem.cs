using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum CatalysItemType
{
    red,
    blue,
    green,
    black,
    white,
}

public class ExecCatalysItem : ExecutableItem
{
    public CatalysItemType ItemType;
    public ExecCatalysItem(CatalysItemType type, int count = 1) 
        : base(ExecutableType.catalys, count,Formuls.CostCatalys(type))
    {

        IconSprite = UnityEngine.Resources.Load<Sprite>("sprites/Catalys/" + type.ToString());
        ItemType = type;
        name = type.ToString();
    }
    
    public static ExecCatalysItem Creat()
    {
        return new ExecCatalysItem(ShopController.AllCatalyses.RandomElement());
    }

    public override string Save()
    {
        return base.Save() + DELEM + (int)ItemType;
    }
}

