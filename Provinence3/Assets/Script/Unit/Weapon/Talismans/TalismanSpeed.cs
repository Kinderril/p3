using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanSpeed : Talisman
{

    public override void Use()
    {
        TimeEffect.Creat(MainController.Instance.level.MainHero, EffectType.speed);
        base.Use();
    }
}

