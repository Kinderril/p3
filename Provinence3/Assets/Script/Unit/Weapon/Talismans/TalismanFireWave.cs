using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanFireWave : Talisman
{
    public override void Use()
    {
        var targets2 = Map.Instance.GetEnimiesInRadius(80);
        foreach (var baseMonster in targets2)
        {
            TimeEffect.Creat(baseMonster, EffectType.fire, sourseItem.power,6);
            base.Use();
        }
    }
}

