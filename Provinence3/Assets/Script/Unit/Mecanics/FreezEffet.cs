using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class FreezEffet : TimeEffect
{
    private BaseMonster monster;
    public FreezEffet(Unit targetUnit, float totalTime) 
        : base(targetUnit, totalTime)
    {
        EffectType = EffectType.freez;
        monster = targetUnit as BaseMonster;
        if (monster != null)
        {
            monster.Disable();
        }
    }

    protected override void OnTimer()
    {
        if (monster != null)
        {
            monster.Activate();
        }
        base.OnTimer();
    }
}

