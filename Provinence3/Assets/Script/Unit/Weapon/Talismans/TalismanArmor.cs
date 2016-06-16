using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanArmor : TalismanWithTime
{
    private const float LVL_1_P = 1.3f;
    private const float LVL_10_P = 1.6f;

    private const int LVL_1_T = 5;
    private const int LVL_10_T = 10;
    

    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans, LVL_1_T, LVL_10_T);

        var pointPower =  (LVL_10_P - LVL_1_P) / DiffOfTen();
        power = (LVL_1_P + sourseItem.points * pointPower) * EnchntCoef();
    }
    public override string PowerInfo()
    {
        return "Increase Physical defence:" + (power*10).ToString("0") + "% on " + TimeCoef.ToString("0") + " second";
    }


    public override void Use()
    {
        var trg = MainController.Instance.level.MainHero;
        var timeEffect = new ParameterEffect(trg,TimeCoef,ParamType.PDef, power);
        TimeEffect.Creat(trg, timeEffect);
        base.Use();
    }
}

