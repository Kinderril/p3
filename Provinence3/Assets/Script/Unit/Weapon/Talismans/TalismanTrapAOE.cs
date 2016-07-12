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
        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.enchant);
    }

    public override void Use()
    {
        var p = hero.transform.position;
        var item = DataBaseController.GetItem<AOETrap>( cacheGameObject, p);
        item.Init(Power, true,this);
        base.Use();
    }
    public override string PowerInfo()
    {
        return "Set a trap whitch exposis when some monster come close with delay: " + Trap.WAIT_FOR_EXPLOSION + " and deal: "+ Power.ToString("0") + " Damage to all in radius";
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

