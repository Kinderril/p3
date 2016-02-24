using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum CraftItemType
{
    
}

public class ExecCraftItem : ExecutableItem
{
    public CraftItemType ItemType;
    public ExecCraftItem(CraftItemType type, int count) 
        : base(ExecutableType.craft, count)
    {

        ItemType = type;
    }

    public override string Save()
    {
        return base.Save() + DELEM + (int)ItemType;
    }
}

