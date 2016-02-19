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
    public int count;

    public ExecutableItem(ExecutableType type,int count)
    {
        ExecutableType = type;
        Slot = Slot.executable;
        this.count = count;
    }

    public override string Name {
        get { return ExecutableType.ToString(); } 
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

    public static ExecutableItem Creat(string subStr)
    {
        var spl = subStr.Split(DELEM);
        ExecutableType t = (ExecutableType)Enum.Parse(typeof (ExecutableType), spl[0]);
        
        return new ExecutableItem(t,Convert.ToInt32(spl[1]));
    }
}

