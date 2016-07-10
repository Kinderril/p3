using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TalismanSplitter : Talisman ,IBulletHolder
{
    public const string WAY_CHAIN_BULLET = "BulletSplitter";
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 1.6f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 1.4f - Talisman.LVL_1_AV_MONSTER_HP /1.6f;
    private float dmg ;

    private Bullet cacheGameObject;
    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {

        base.Init(level, sourseItem, countTalismans);
        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.Enchant);

    }
    public TalismanSplitter()
    {
        cacheGameObject = Resources.Load(base_path + WAY_CHAIN_BULLET, typeof(Bullet)) as Bullet;
    }
    public override string PowerInfo()
    {
        return "Casts magical arrors to nearby enemies for : " + power.ToString("0") + " total damage. Damage evenly divided by all enemies";
    }

    public override void Use()
    {
        var closestEnemies = Map.Instance.GetEnimiesInRadius(70);
        if (closestEnemies.Count > 0)
        {
            dmg = power/ closestEnemies.Count;
            foreach (var closestEnemy in closestEnemies)
            {
                var bullet = DataBaseController.GetItem<Bullet>(cacheGameObject);
                bullet.transform.position = hero.weaponsContainer.position;
                bullet.Init(closestEnemy, this, bullet.transform.position);
            }
            base.Use();
        }
    }

    public SpecialAbility SpecAbility
    {
        get { return SpecialAbility.none;}
    }

    public float Power
    {
        get { return dmg; }
    }
    public Unit Owner
    {
        get { return hero; }
    }
    public WeaponType DamageType
    {
        get {return WeaponType.magic;} 
    }
}

