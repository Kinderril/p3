using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Ammo
{
    public int CurAmmo { get; private set; }
    private Action<ItemId, int> activaAction;
    private Action TriggerZeroAmmo;

    public Ammo(Action<ItemId, int> activaAction,Action triggerZeroAmmo)
    {
        this.activaAction = activaAction;
        this.TriggerZeroAmmo = triggerZeroAmmo;
    }

    public void DoShoot()
    {
        CurAmmo--;
        activaAction(ItemId.ammo, -1);
    }

    public bool CanShoot()
    {
        bool b = CurAmmo > 0;
        if (!b)
        {
            if (TriggerZeroAmmo != null)
            {
                TriggerZeroAmmo();
            }
        }
        return b;
    }

    public void AddAmmo(int ammo)
    {
        CurAmmo += ammo;
        activaAction(ItemId.ammo, ammo);
    }
}

