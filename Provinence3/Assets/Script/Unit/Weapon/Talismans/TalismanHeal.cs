using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanHeal : Talisman
{
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 3.1f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 3.2f;
    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans);

        var pointPower = (LVL_10_P - LVL_1_P) /DiffOfTen();

        power = sourseItem.points*pointPower * EnchntCoef();
    }

    public override void Use()
    {
        MainController.Instance.level.MainHero.GetHeal(power);
        base.Use();
    }
}

