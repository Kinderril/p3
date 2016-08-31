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
        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.enchant);

    }
    public override string PowerInfo()
    {
        return "Increase outcoming hero damage by " + (10 * power).ToString("0") + "% for " + TimeCoef.ToString("0") + " sec.";
    }

    protected override void Use()
    {
        TimeEffect.Creat(MainController.Instance.level.MainHero, EffectType.doubleDamage,power,TimeCoef);
        base.Use();
    }
}

