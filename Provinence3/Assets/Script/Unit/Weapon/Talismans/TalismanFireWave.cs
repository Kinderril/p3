using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanFireWave : TalismanWithTime
{
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 2.5f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 2.4f - Talisman.LVL_1_AV_MONSTER_HP / 2.5f;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans,8,2);
        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.enchant) /TimeCoef;
    }
    public override string PowerInfo()
    {
        return "Fires nearby enemies for " + TimeCoef.ToString("0") + " sec. Dealing " + power.ToString("0") + " every second." ;
    }

    protected override void Use()
    {
        var targets2 = Map.Instance.GetEnimiesInRadius(80);
        if (targets2.Count > 0)
        {
            foreach (var baseMonster in targets2)
            {
                TimeEffect.Creat(baseMonster, EffectType.fire, power, TimeCoef);
            }
            base.Use();
        }
    }
}

