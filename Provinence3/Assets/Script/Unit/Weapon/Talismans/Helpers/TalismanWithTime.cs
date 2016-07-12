using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanWithTime : Talisman
{
    protected float TimeCoef = 10;
    protected const float TRAP_1_LVL_TIME = 8;
    protected const float TRAP_10_LVL_TIME = 3;
    public void Init(Level level, TalismanItem sourseItem, int countTalismans,float lvl_1_time, float offsetToLvl10)
    {
        base.Init(level, sourseItem, countTalismans);
        TimeCoef = Formuls.PowerTalicStandart(lvl_1_time, offsetToLvl10, sourseItem.points, sourseItem.enchant);
    }
}

