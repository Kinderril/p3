using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanSplitter : Talisman
{
    private const float LVL_1_P = 30f;
    private const float LVL_10_P = 121f;

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

