using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanMassFreez : Talisman
{
    //TODO calculates
    protected override void Use()
    {
        var targets = Map.Instance.GetEnimiesInRadius(80);
        foreach (var baseMonster in targets)
        {
            TimeEffect.Creat(baseMonster, EffectType.freez, 0 , 2);
        }
        base.Use();
    }
}

