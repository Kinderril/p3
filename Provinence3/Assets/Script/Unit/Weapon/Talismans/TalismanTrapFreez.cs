using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanTrapFreez : TalismanWithTime ,IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "";
    private AOETrap cacheGameObject;
    private const float LVL_1_P = 4f;
    private const float LVL_10_P = 6;
    public TalismanTrapFreez()
    {
        cacheGameObject = Resources.Load(WAY_CHAIN_BULLET, typeof(AOETrap)) as AOETrap;
    }

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans, LVL_1_P, LVL_10_P);
        var pointPower = (LVL_10_P - LVL_1_P) / DiffOfTen();
        power = sourseItem.points * pointPower * EnchntCoef();
    }

    public override void Use()
    {
        var p = hero.transform.position;
        var item = DataBaseController.GetItem<AOETrap>(cacheGameObject, p);
        item.Init(Power, false, this);
        base.Use();
    }
    public SpecialAbility SpecAbility
    {
        get { return SpecialAbility.none; }
    }
    public float Power
    {
        get { return power; }
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

