using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PhysicHeroWeapon : Weapon
{
    public override void DoShoot(Vector3 v, float additionalPower = 0, Unit target = null)
    {
        base.DoShoot(v);
        MainController.Instance.level.Ammo.DoShoot();
    }
    public override bool CanShoot()
    {
        return  base.CanShoot() && MainController.Instance.level.Ammo.CanShoot();
    }
}

