﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AOETrap : Trap
{
    public bool isDamage = false;
    protected override void DoAction()
    {
        foreach (var monster in monstersInside)
        {
            if (isDamage)
            {
                monster.GetHit(power, WeaponType.physics);
            }
            else
            {
                TimeEffect.Creat(monster, EffectType.freez);
            }
        }
    }
}

