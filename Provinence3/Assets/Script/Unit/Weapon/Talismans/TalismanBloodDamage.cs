using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanBloodDamage : Talisman , IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "";
    private Bullet cacheGameObject;
    public TalismanBloodDamage(TalismanItem sourseItem, int countTalismans) 
        : base(sourseItem, countTalismans)
    {
        cacheGameObject = Resources.Load(WAY_CHAIN_BULLET, typeof(Bullet)) as Bullet;
    }
    public override void Use()
    {
        var bullet = DataBaseController.GetItem<Bullet>(cacheGameObject);

        hero.GetHit(Power/3,WeaponType.magic);
        bullet.Init(GetClosestMonster(),this);

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

