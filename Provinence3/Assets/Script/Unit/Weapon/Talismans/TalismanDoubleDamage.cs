using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanDoubleDamage : Talisman
{
    public override void Use()
    {
        TimeEffect.Creat(MainController.Instance.level.MainHero, EffectType.doubleDamage,0, sourseItem.power);
        base.Use();
    }
}

