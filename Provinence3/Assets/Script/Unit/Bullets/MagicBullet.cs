using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MagicBullet : HeroBullet
{
    private Weapon weapon;
    public override void Init(Vector3 direction, Weapon weapon)
    {
        base.Init(direction, weapon);
        this.weapon = weapon;
    }

    protected override Vector3 FindStartPos(Weapon weapon)
    {
        if (AffecttedUnits.Count > 0)
        {
            return transform.position;
        }
        return base.FindStartPos(weapon);
    }

    protected override void Hit(Unit unit)
    {
        base.Hit(unit);
        if (IsUsing)
        {
            FindNextTarget(unit);
        }
    }

    public BaseMonster GetClosestMonster()
    {
        return Map.Instance.FindClosesEnemy(transform.position,40,AffecttedUnits);
    }
    private void FindNextTarget(Unit lastTrg)
    {
        var target = GetClosestMonster();
        if (target != null)
        {
            var dir = target.transform.position - transform.position;
            Init(dir, weapon);
        }
        else
        {
            Death(lastTrg);
        }
    }
}

