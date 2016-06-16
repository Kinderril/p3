using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanBloodDamage : Talisman , IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "BulletBloodDamage";

    private const float LVL_1_P = 40f;
    private const float LVL_10_P = 150f;

    private const int LVL_1_S = 15;
    private const int LVL_10_S = 43;

    private float SefDmg = 10;

    private Bullet cacheGameObject;
    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans);

        var pointPower = (LVL_10_P - LVL_1_P) / DiffOfTen();
        power = sourseItem.power * pointPower * EnchntCoef();

        var sefl = (LVL_10_S - LVL_1_S) / DiffOfTen();
        SefDmg = sourseItem.power*sefl;
    }

    public TalismanBloodDamage() 
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(Bullet)) as Bullet;
    }
    public override void Use()
    {
        var bullet = DataBaseController.GetItem<Bullet>(cacheGameObject);

        hero.GetHit(SefDmg, WeaponType.magic);
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

