using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum CatalysItemType
{
    penetrating,
    AOE,
    Critical,
    homing,
    push,
    slow,
    removeDefence,
    vampire,
    chain,
    clear,
    dot,
    stun,
    distance,
    hp,
    shield,
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

    public SpecialAbility GetSpec()
    {
        return (SpecialAbility)Enum.Parse(typeof(SpecialAbility), ItemType.ToString(), true);
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

