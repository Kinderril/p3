using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


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
    float Range
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

    Transform BulletComeOut
    {
        get;
    }
    Transform Transform
    {
        get;
    }
}

