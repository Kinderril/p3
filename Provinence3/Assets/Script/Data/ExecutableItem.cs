using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ExecutableType
{
    weaponUpdate,
    powerUpdate,
    armorUpdate,
    healthUpdate,
    
}
public class ExecutableItem : BaseItem
{
    public ExecutableType ExecutableType;
    public const char FIRSTCHAR = '©';

    public ExecutableItem(ExecutableType type)
    {
        ExecutableType = type;
        Slot = Slot.executable;
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
        return ExecutableType.ToString();
    }

    public static ExecutableItem Creat(string subStr)
    {
        ExecutableType t = (ExecutableType)Enum.Parse(typeof (ExecutableType), subStr);
        return new ExecutableItem(t);
    }
}

