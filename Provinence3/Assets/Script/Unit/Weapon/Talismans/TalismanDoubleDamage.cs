using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanDoubleDamage : TalismanWithTime
{

    private const float LVL_1_P = 1.4f;
    private const float LVL_10_P = 0.6f;

    private const int LVL_1_T = 5;
    private const int LVL_10_T = 5;

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans, LVL_1_T, LVL_10_T);
        var pointPower = (LVL_10_P) / DiffOfTen();
        power =(LVL_1_P +  sourseItem.points * pointPower) * EnchntCoef();

    }

    public override void Use()
    {
        TimeEffect.Creat(MainController.Instance.level.MainHero, EffectType.doubleDamage,power,TimeCoef);
        base.Use();
    }
}

