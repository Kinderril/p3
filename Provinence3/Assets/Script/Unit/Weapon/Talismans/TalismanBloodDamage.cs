using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanBloodDamage : Talisman , IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "BulletBloodDamage";

    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP/1.2f;
    private float LVL_10_P = (Talisman.LVL_10_AV_MONSTER_HP- LVL_1_AV_MONSTER_HP )/ 1.1f;

    private const int LVL_1_S = 15;
    private const int LVL_10_S = 29;

    private float SefDmg = 10;

    private Bullet cacheGameObject;
    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans);

        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.enchant);

        var sefl = (LVL_10_S ) / Formuls.DiffOfTen();
        SefDmg = LVL_1_S +  sourseItem.points * sefl;
    }

    public TalismanBloodDamage() 
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(Bullet)) as Bullet;
    }
    protected override void Use()
    {
        var closestMonster = GetClosestMonster();
        if (closestMonster != null)
        {
            var bullet = DataBaseController.GetItem<Bullet>(cacheGameObject);
            hero.GetHit(SefDmg, WeaponType.magic, new DeathInfo(SefDmg, WeaponType.magic, SourceType.talisman));
            bullet.transform.position = hero.weaponsContainer.position;
            bullet.Init(closestMonster, this, bullet.transform.position);
            base.Use();
        }
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
    public override string PowerInfo()
    {
        return "Casts to nearby enemy and with power: " + Power.ToString("0") + ". But Dealing " + SefDmg.ToString("0") + " to caster";
    }


    public WeaponType DamageType
    {
        get { return WeaponType.magic; }
    }
}

