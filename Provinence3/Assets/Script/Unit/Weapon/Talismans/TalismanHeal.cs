using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanHeal : Talisman
{
    private float LVL_1_P = Talisman.LVL_1_AV_MONSTER_HP / 3.1f;
    private float LVL_10_P = Talisman.LVL_10_AV_MONSTER_HP / 3.2f - Talisman.LVL_1_AV_MONSTER_HP / 3.1f;
    public override void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        base.Init(level, sourseItem, countTalismans);
        power = Formuls.PowerTalicStandart(LVL_1_P, LVL_10_P, sourseItem.points, sourseItem.enchant);
    }
    public override string PowerInfo()
    {
        return "Healing hero by: " + power.ToString("0");
    }

    public override void Use()
    {
        MainController.Instance.level.MainHero.GetHeal(power);
        base.Use();
    }
}

