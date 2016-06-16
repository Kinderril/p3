using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanWithTime : Talisman
{
    protected float TimeCoef = 10;
    protected const float TRAP_1_LVL_TIME = 8;
    protected const float TRAP_10_LVL_TIME = 11;
    public void Init(Level level, TalismanItem sourseItem, int countTalismans,float lvl_1_time, float lvl_10_time)
    {
        base.Init(level, sourseItem, countTalismans);

        var timePower = (lvl_10_time - lvl_1_time) / DiffOfTen();
        TimeCoef = sourseItem.points * timePower * EnchntCoef();
    }
}

