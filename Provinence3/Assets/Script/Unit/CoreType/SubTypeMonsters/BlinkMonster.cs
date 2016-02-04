using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BlinkMonster : BaseMonster
{
    protected override void StartAttack()
    {
        StartBlink();
        base.StartAttack();
    }

    private void StartBlink()
    {
        var trg = MainController.Instance.level.MainHero.transform.position;

    }
}

