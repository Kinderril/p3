using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanBloodDamage : Talisman , IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "BulletBloodDamage";

    private Bullet cacheGameObject;
    public TalismanBloodDamage() 
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(Bullet)) as Bullet;
    }
    public override void Use()
    {
        var bullet = DataBaseController.GetItem<Bullet>(cacheGameObject);

        hero.GetHit(Power/3,WeaponType.magic);
        var closestMonster = GetClosestMonster();
        bullet.transform.position = hero.weaponsContainer.position;
        bullet.Init(closestMonster, this,hero.transform.position);

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

