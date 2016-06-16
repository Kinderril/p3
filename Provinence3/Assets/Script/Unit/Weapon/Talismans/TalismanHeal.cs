using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanHeal : Talisman
{
    private const int LVL_1 = 30;
    private const int LVL_10 = 100;
    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans);

        var pointPower = (LVL_10 - LVL_1) /DiffOfTen();

        power = sourseItem.power*pointPower * EnchntCoef();
    }

    public override void Use()
    {
        MainController.Instance.level.MainHero.GetHeal(power);
        base.Use();
    }
}

