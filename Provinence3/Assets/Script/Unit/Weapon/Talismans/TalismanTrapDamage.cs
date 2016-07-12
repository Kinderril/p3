using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanTrapDamage : TalismanWithTime ,IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "SingleTrap";
    private IncomingTrap cacheGameObject;
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 4.6f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 4.5f - Talisman.LVL_1_AV_MONSTER_HP / 4.6f;
    public TalismanTrapDamage()
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(IncomingTrap)) as IncomingTrap;
    }

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans,TRAP_1_LVL_TIME,TRAP_10_LVL_TIME);

        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.enchant);
    }

    public override void Use()
    {
        var p= hero.transform.position;
        var item = DataBaseController.GetItem<IncomingTrap>(cacheGameObject, p);
        item.Init(Power,this);
        base.Use();
    }
    public override string PowerInfo()
    {
        return "Set a trap whitch shoots to monster come close for: " + Power.ToString("0") + " damage. LigeTime is:" + LifeTimeTrap.LIFE_TIME;
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

