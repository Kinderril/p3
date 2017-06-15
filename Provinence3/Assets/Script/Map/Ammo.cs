using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Ammo
{
    public int curAmmon;
    private Action<ItemId, int> activaAction;

    public Ammo(Action<ItemId, int> activaAction)
    {
        this.activaAction = activaAction;
    }

    public void DoShoot()
    {
        curAmmon--;
        activaAction(ItemId.ammo, curAmmon);
    }

    public bool CanShoot()
    {
        return curAmmon > 0;
    }

    public void AddAmmo(int ammo)
    {
        curAmmon += ammo;
        activaAction(ItemId.ammo, curAmmon);
    }
}

