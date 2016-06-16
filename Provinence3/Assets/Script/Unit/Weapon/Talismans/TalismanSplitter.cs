using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanSplitter : Talisman
{
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 4.1f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 4.0f;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {

        var pointPower = (LVL_10_P - LVL_1_P) / DiffOfTen();
        power = sourseItem.power * pointPower * EnchntCoef();

        base.Init(level, sourseItem, countTalismans);
    }

    public override void Use()
    {
        //TODO all
        base.Use();
    }
}

