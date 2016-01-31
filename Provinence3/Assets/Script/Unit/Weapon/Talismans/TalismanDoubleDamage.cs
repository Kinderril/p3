using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanDoubleDamage : Talisman
{
    public TalismanDoubleDamage(TalismanItem sourseItem, int countTalismans) : base(sourseItem, countTalismans)
    {
    }
    public override void Use()
    {
        TimeEffect.Creat(MainController.Instance.level.MainHero, EffectType.doubleDamage);
        base.Use();
    }
}

