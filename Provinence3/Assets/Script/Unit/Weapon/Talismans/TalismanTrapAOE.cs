﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanTrapAOE : Talisman , IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "AOETrap";
    private AOETrap cacheGameObject;
    public TalismanTrapAOE(TalismanItem sourseItem, int countTalismans) 
        : base(sourseItem, countTalismans)
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(AOETrap)) as AOETrap;
    }
    public override void Use()
    {
        var p = hero.transform.position;
        var item = DataBaseController.GetItem<AOETrap>( cacheGameObject, p);
        item.Init(sourseItem.power, true,this);
        base.Use();
    }
    public SpecialAbility SpecAbility
    {
        get { return SpecialAbility.none; }
    }
    public float Power
    {
        get { return sourseItem.power; }
    }

    public Unit Owner
    {
        get { return hero; }
    }

    public WeaponType DamageType
    {
        get { return WeaponType.magic; }
    }
}

