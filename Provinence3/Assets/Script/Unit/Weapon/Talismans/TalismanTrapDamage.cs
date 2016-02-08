using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanTrapDamage : Talisman ,IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "SingleTrap";
    private IncomingTrap cacheGameObject;
    public TalismanTrapDamage(TalismanItem sourseItem, int countTalismans) 
        : base(sourseItem, countTalismans)
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(IncomingTrap)) as IncomingTrap;
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
        get { return sourseItem.power; }
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

