using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum EnchantType
{
    weaponUpdate,
    powerUpdate,
    armorUpdate,
//    healthUpdate,
}
public  class ExecEnchantItem : ExecutableItem
{
    public EnchantType ItemType;
    public ExecEnchantItem(EnchantType type, int count) 
        : base(ExecutableType.enchant, count,Formuls.CostEnchant(type,MainController.Instance.PlayerData.Level))
    {
        IconSprite = UnityEngine.Resources.Load<Sprite>("sprites/Enchant/" + type.ToString());
        ItemType = type;
        name = NameOfEnchant(type);
    }

    public static string NameOfEnchant(EnchantType type)
    {
        switch (type)
        {
            case EnchantType.weaponUpdate:
                return "Weapon upgrade";
            case EnchantType.powerUpdate:
                return "Power enchante";
            case EnchantType.armorUpdate:
                return "Armor upgrade";
        }
        return "";
    }
    
    public static ExecEnchantItem Creat()
    {
        return new ExecEnchantItem(ShopController.AllEnchantes.RandomElement(),1);
    }

    public override string Save()
    {
        return base.Save() + DELEM + (int)ItemType;
    }
}
