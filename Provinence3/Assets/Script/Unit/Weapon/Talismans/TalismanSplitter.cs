using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanSplitter : Talisman
{
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 4.1f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 4.0f - Talisman.LVL_1_AV_MONSTER_HP / 4.1f;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {

        base.Init(level, sourseItem, countTalismans);
        var pointPower = (LVL_10_P) / DiffOfTen();
        power = (LVL_1_P + sourseItem.points * pointPower) * EnchntCoef();

    }

    public override void Use()
    {
        //TODO all
        base.Use();
    }
}

