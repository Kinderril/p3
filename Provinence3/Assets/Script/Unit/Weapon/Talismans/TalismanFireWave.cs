using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanFireWave : Talisman
{
    public TalismanFireWave(TalismanItem sourseItem, int countTalismans) : base(sourseItem, countTalismans)
    {
    }
    public override void Use()
    {
        var targets2 = Map.Instance.GetEnimiesInRadius(80);
        foreach (var baseMonster in targets2)
        {
            TimeEffect.Creat(baseMonster, EffectType.fire, sourseItem.power);
            base.Use();
        }
    }
}

