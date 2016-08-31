using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanArmor : TalismanWithTime
{
    private const float LVL_1_P = 1.3f;
    private const float LVL_10_P = 0.3f;

    private const int LVL_1_T = 5;
    private const int LVL_10_T = 5;
    

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans, LVL_1_T, LVL_10_T);
        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.enchant);
    }
    public override string PowerInfo()
    {
        return "Increase Physical defence:" + (power*10).ToString("0") + "% on " + TimeCoef.ToString("0") + " second";
    }

    protected override void Use()
    {
        base.Use();
        var trg = MainController.Instance.level.MainHero;
        var timeEffect = new ParameterEffect(trg,TimeCoef,ParamType.PDef, power);
        TimeEffect.Creat(trg, timeEffect);
    }
}

