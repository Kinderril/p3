using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanFireWave : TalismanWithTime
{

    private const float LVL_1_P = 50f;
    private const float LVL_10_P = 180f;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans,8,10);
        var pointPower = (LVL_10_P - LVL_1_P) / DiffOfTen();
        power = sourseItem.power * pointPower * EnchntCoef();

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

