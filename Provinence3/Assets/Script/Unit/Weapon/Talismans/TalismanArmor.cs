using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TalismanArmor : Talisman
{
    public override void Use()
    {
        var trg = MainController.Instance.level.MainHero;
        var timeEffect = new ParameterEffect(trg,sourseItem.power,ParamType.PDef,1.5f);
        TimeEffect.Creat(trg, timeEffect);
        base.Use();
    }
}

