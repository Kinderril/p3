using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ParamType
{
    Speed,
    MPower,
    PPower,
    PDef,
    MDef,
    Hp,

}
public class UnitParameters : ScriptableObject
{
    public Dictionary<ParamType, float> Parameters; 

    public float Speed = 5;
    public float MPower = 1;
    public float PPower = 1;
    public int MaxHp = 15;
    public float magicResist = 1f;
    public float physicResist = 1f;
    public int Level = 1;
    public AttackType AttackType;
    
    public UnitParameters Copy()
    {
        var p = CreateInstance(typeof(UnitParameters)) as UnitParameters;
        p.Speed = Speed;
        p.MPower = MPower;
        p.PPower = PPower;
        p.MaxHp = MaxHp;
        p.magicResist = magicResist;
        p.physicResist = physicResist;
        p.Parameters= new Dictionary<ParamType, float>();
        p.Parameters.Add(ParamType.Speed,Speed );
        p.Parameters.Add(ParamType.MPower, MPower);
        p.Parameters.Add(ParamType.PPower, PPower);
        p.Parameters.Add(ParamType.PDef, physicResist);
        p.Parameters.Add(ParamType.MDef, magicResist);
        p.Parameters.Add(ParamType.Hp,MaxHp );
        p.AttackType = AttackType;
        return p;

    }
}

