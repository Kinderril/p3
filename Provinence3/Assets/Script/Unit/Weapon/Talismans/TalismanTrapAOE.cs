using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanTrapAOE : TalismanWithTime , IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "AOETrap";
    private AOETrap cacheGameObject;
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 4.1f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 4.0f - Talisman.LVL_1_AV_MONSTER_HP / 4.1f;
    public TalismanTrapAOE()
        :base()
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(AOETrap)) as AOETrap;
    }

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans,TRAP_1_LVL_TIME,TRAP_10_LVL_TIME);
        var pointPower = (LVL_10_P ) / DiffOfTen();
        power = (LVL_1_P + sourseItem.points * pointPower) * EnchntCoef();
    }

    public override void Use()
    {
        var p = hero.transform.position;
        var item = DataBaseController.GetItem<AOETrap>( cacheGameObject, p);
        item.Init(Power, true,this);
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

