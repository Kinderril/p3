﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanTrapDamage : TalismanWithTime ,IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "SingleTrap";
    private IncomingTrap cacheGameObject;
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 3.6f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 3.5f - Talisman.LVL_1_AV_MONSTER_HP / 3.6f;
    public TalismanTrapDamage()
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(IncomingTrap)) as IncomingTrap;
    }

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans,TRAP_1_LVL_TIME,TRAP_10_LVL_TIME);

        var pointPower = (LVL_10_P ) / DiffOfTen();
        power = (LVL_1_P + sourseItem.points * pointPower )* EnchntCoef();
    }

    public override void Use()
    {
        var p= hero.transform.position;
        var item = DataBaseController.GetItem<IncomingTrap>(cacheGameObject, p);
        item.Init(Power,this);
        base.Use();
    }

    public SpecialAbility SpecAbility
    {
        get { return SpecialAbility.none; }
    }
    public float Power {
        get { return power; }
    }

    public Unit Owner
    {
        get { return hero; }
    }

    public WeaponType DamageType
    {
        get { return WeaponType.physics; }
    }
}

