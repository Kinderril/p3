using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.iOS;
using UnityEngine;

public enum CraftItemType
{
    Iron,
    Leather,
    Coal,
    Thread,
    Bone,
    Stem,
    Oil,
    Silver,
    Hardner,
    Splinter,
}

public class ExecCraftItem : ExecutableItem
{
    public CraftItemType ItemType;
    public ExecCraftItem(CraftItemType type, int count,bool loadSprite = true) 
        : base(ExecutableType.craft, count)
    {
        if (loadSprite)
        {
            IconSprite = UnityEngine.Resources.Load<Sprite>("sprites/CraftItems/" + type.ToString());
        }
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

