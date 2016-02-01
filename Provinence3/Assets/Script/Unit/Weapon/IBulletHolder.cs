using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IBulletHolder
{
    SpecialAbility SpecAbility
    {
        get;
    }

    float Power
    {
        get;
    }
    Unit Owner 
    {
        get;
    }
    WeaponType DamageType
    {
        get;
    }
}

