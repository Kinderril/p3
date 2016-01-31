using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanMassFreez : Talisman
{
    public TalismanMassFreez(TalismanItem sourseItem, int countTalismans) 
        : base(sourseItem, countTalismans)
    {

    }
    public override void Use()
    {
        var targets = Map.Instance.GetEnimiesInRadius(80);
        foreach (var baseMonster in targets)
        {
            TimeEffect.Creat(baseMonster, EffectType.freez);
        }
        base.Use();
    }
}

