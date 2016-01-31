using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MagicHeroWeapon : Weapon
{
    public override float GetPower()
    {
        return owner.Parameters.Parameters[ParamType.MPower] + 0.5f*owner.Parameters.Parameters[ParamType.PPower];
    }

    public override void DoShoot(Vector3 v, Unit target = null)
    {
        base.DoShoot(v);
        MainController.Instance.level.AddItem(ItemId.energy, 1);
    }
}

