using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ExecutableType
{
    craft,
    enchant,
    catalys,
    recipe,
}

public class ExecutableItem : BaseItem
{
    public ExecutableType ExecutableType;
    public const char FIRSTCHAR = '©';
    public int count;

    public ExecutableItem(ExecutableType type,int count)
    {
        ExecutableType = type;
        Slot = Slot.executable;
        this.count = count;
    }

    public override char FirstChar()
    {
        return FIRSTCHAR;
    }

    public override void Activate(Hero hero)
    {
        Debug.LogWarning("cannot be activate");
    }

    public override string Save()
    {
        return ExecutableType.ToString() + DELEM + count;
    }

    public static ExecutableItem Create(string subStr)
    {
        var spl = subStr.Split(DELEM);
        ExecutableType t = (ExecutableType)Enum.Parse(typeof (ExecutableType), spl[0]);
        ExecutableItem item = null;
        var count = Convert.ToInt32(spl[1]);
        switch (t)
        {
            case ExecutableType.craft:

                item = new ExecCraftItem((CraftItemType)Enum.Parse(typeof(CraftItemType), spl[2]), count);
                break;
            case ExecutableType.enchant:

                break;
            case ExecutableType.catalys:

                break;
        }

        return item;
    }
}

