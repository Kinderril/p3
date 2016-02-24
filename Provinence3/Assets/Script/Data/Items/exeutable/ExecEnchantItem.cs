using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum EnchantType
{
    weaponUpdate,
    powerUpdate,
    armorUpdate,
    healthUpdate,
}
public  class ExecEnchantItem : ExecutableItem
{
    public EnchantType ItemType;
    public ExecEnchantItem(EnchantType type, int count) 
        : base(ExecutableType.enchant, count)
    {
        ItemType = type;
    }
    public override string Save()
    {
        return base.Save() + DELEM + (int)ItemType;
    }
}
