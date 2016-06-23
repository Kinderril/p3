using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanSplitter : Talisman
{
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 1.6f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 1.4f - Talisman.LVL_1_AV_MONSTER_HP /1.6f;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {

        base.Init(level, sourseItem, countTalismans);
        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.Enchant);

    }
    public override string PowerInfo()
    {
        return "Casts magical arrors to nearby enemies for : " + power.ToString("0") + " total damage. Damage evenly divided by all enemies";
    }

    public override void Use()
    {
        //TODO all
        base.Use();
    }
}

