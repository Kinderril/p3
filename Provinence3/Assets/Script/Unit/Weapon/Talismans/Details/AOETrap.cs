using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class AOETrap : Trap
{
    public bool isDamage = false;

    public void Init(float power,bool isDamage)
    {
        Init(power);
        this.isDamage = isDamage;
    }
    protected override void DoAction()
    {
        foreach (var monster in monstersInside)
        {
            if (isDamage)
            {
                monster.GetHit(power, WeaponType.physics);
            }
            else
            {
                TimeEffect.Creat(monster, EffectType.freez);
            }
        }
    }
}

