using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AOEBullet : HeroBullet
{
    public bool StayOnPosition;
    public override void Init(Vector3 target, Weapon weapon)
    {
        base.Init(target, weapon);
        if (StayOnPosition)
        {
            start = target;
        }
        else
        {

            start = new Vector3(target.x, target.y + 5, target.z);
        }
    }

    protected override void Hit(Unit unit)
    {
        if (!AffecttedUnits.Contains(unit))
        {
            AffecttedUnits.Add(unit);
            unit.GetHit(this);
        }
    }
}

