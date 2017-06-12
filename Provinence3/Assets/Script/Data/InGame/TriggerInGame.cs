using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class TriggerInGame
{
    private BaseSpell spell;//TODO spell in game
    private int chargesRemain = 0;

    public TriggerInGame(BaseSpell spell)
    {
        chargesRemain = spell.BaseTrigger.ShootCount;//TODO
    }

    public void Activate()
    {
        chargesRemain--;
        if (chargesRemain <= 0)
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        
    }
}

