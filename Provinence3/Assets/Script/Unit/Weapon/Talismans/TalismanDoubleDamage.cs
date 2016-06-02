using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanDoubleDamage : Talisman
{
    public override void Use()
    {
        //TODO time to power
        TimeEffect.Creat(MainController.Instance.level.MainHero, EffectType.doubleDamage);
        base.Use();
    }
}

