﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanFireWave : TalismanWithTime
{

    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 2.5f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 2.4f;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans,8,10);
        var pointPower = (LVL_10_P - LVL_1_P) / DiffOfTen();
        power = sourseItem.points * pointPower * EnchntCoef();

    }

    public override void Use()
    {
        var targets2 = Map.Instance.GetEnimiesInRadius(80);
        foreach (var baseMonster in targets2)
        {
            TimeEffect.Creat(baseMonster, EffectType.fire, power, TimeCoef);
            base.Use();
        }
    }
}

