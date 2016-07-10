using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum CraftItemType
{
    Iron,
    Wood,
    Leather,
    Thread,

    Bone,
    Mercury,
    Gems,
    Silver,

    Splinter,
}

public class ExecCraftItem : ExecutableItem
{
    public CraftItemType ItemType;
    public ExecCraftItem(CraftItemType type, int count) 
        : base(ExecutableType.craft, count,Formuls.CostCraftItemType(type))
    {
        IconSprite = DataBaseController.Instance.CraftItemSprite(type);
        ItemType = type;
        name = type.ToString();
    }
    public override string Name
    {
        get { return name; }
    }

    public override string Save()
    {
        return base.Save() + DELEM + (int)ItemType;
    }


}

