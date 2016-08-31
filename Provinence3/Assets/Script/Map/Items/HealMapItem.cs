using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class HealMapItem : BaseMapItem
{
    protected override void Take(Hero unit)
    {
        var val = unit.Parameters[ParamType.Heath]*0.2f;
        unit.GetHeal(val);
    }
}

