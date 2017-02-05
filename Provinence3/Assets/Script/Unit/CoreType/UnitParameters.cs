using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ParamType
{
    PPower,
    MPower,
    PDef,
    MDef,
    Heath,
    Speed,

}

[Serializable]
public struct DropItem
{
    public const float CHANCE_SIMPLE_0_1 = 0.45f;
    public const float CHANCE_RARE_0_1 = 0.65f;

    public float chance0_1_simple;
    public float chance0_1_rare;
    public CraftItemType type_simple;
    public CraftItemType type_rare;
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
    public DropItem DropItem;

    public UnitParameters Copy()
    {
        var p = CreateInstance(typeof(UnitParameters)) as UnitParameters;
        p.Speed = Speed;
        p.MPower = MPower;
        p.PPower = PPower;
        p.MaxHp = MaxHp;
        p.magicResist = magicResist;
        p.DropItem = DropItem;
        p.physicResist = physicResist;
        p.Parameters= new Dictionary<ParamType, float>();
        p.Parameters.Add(ParamType.Speed,Speed );
        p.Parameters.Add(ParamType.MPower, MPower);
        p.Parameters.Add(ParamType.PPower, PPower);
        p.Parameters.Add(ParamType.PDef, physicResist);
        p.Parameters.Add(ParamType.MDef, magicResist);
        p.Parameters.Add(ParamType.Heath,MaxHp );
        p.AttackType = AttackType;
        return p;

    }
    public UnitParametersInGame Get()
    {
        var p = new UnitParametersInGame();
        p.MaxHp = MaxHp;
        p.AttackType = AttackType;
        p.Level = Level;
        p.Add(ParamType.Speed, Speed);
        p.Add(ParamType.MPower, MPower);
        p.Add(ParamType.PPower, PPower);
        p.Add(ParamType.PDef, physicResist);
        p.Add(ParamType.MDef, magicResist);
        p.Add(ParamType.Heath, MaxHp);
        p.SimpleDrop = OtherType(DropItem.type_simple,false);
        p.RareDrop = OtherType(DropItem.type_rare,true);

        return p;
    }

    private List<CraftItemType> OtherType(CraftItemType t,bool isRare)
    {
        var count = Enum.GetValues(typeof (CraftItemType)).Length;
        List < CraftItemType > list = new List<CraftItemType>();
        foreach (CraftItemType v in Enum.GetValues(typeof(CraftItemType)))
        {
            var rare = false;
            if (isRare)
            {
                rare = (int) v >= count/2;
            }
            else
            {
                rare = (int)v < count / 2;
            }
            if (v != t && rare)
            {
                list.Add(v);
            }
        }
        return list;
    }
}

public class UnitParametersInGame : Dictionary<ParamType, float>
{
    public List<CraftItemType> SimpleDrop;
    public List<CraftItemType> RareDrop;
    public float MaxHp;
    public int Level = 1;
    public AttackType AttackType;
}


