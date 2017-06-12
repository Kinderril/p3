using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

//public enum ParamType
//{
//    PPower,
//    MPower,
//    PDef,
//    MDef,
//    Heath,
//    Speed,
//
//}

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
        var d = new Dictionary<ParamType,float>();
        d.Add(ParamType.Speed, Speed);
        d.Add(ParamType.MPower, MPower);
        d.Add(ParamType.PPower, PPower);
        d.Add(ParamType.PDef, physicResist);
        d.Add(ParamType.MDef, magicResist);
        d.Add(ParamType.Heath, MaxHp);
        var p = new UnitParametersInGame(d);
        p.MaxHp = MaxHp;
        p.AttackType = AttackType;
        p.Level = Level;
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

public class UnitParametersInGame// : Dictionary<ParamType, float>
{
    public List<CraftItemType> SimpleDrop;
    public List<CraftItemType> RareDrop;
    private float _maxHp;
    public float MaxHp
    {
        get { return _maxHp; }
        set
        {
            _maxHp = value;
            baseParams[ParamType.Heath] = _maxHp;
        }
    }

    public int Level = 1;
    public AttackType AttackType;

    private Dictionary<ParamType, float> baseParams = new Dictionary<ParamType, float>();
    private Dictionary<ParamType, float> coefParams = new Dictionary<ParamType, float>();
    private Dictionary<ParamType, float> effectParams = new Dictionary<ParamType, float>();

    public UnitParametersInGame(Dictionary<ParamType, float> startParams)
    {
        foreach (ParamType p in Enum.GetValues(typeof(ParamType)))
        {
            baseParams.Add(p, startParams[p]);
            coefParams.Add(p,1f);
            effectParams.Add(p,0f);
        }
    }

    private float Get(ParamType t)
    {
        return (baseParams[t] + effectParams[t])* coefParams[t];
    }

    public void Add(ParamType t, float v)
    {
        effectParams[t] += v;
    }
    public void Remove(ParamType t, float v)
    {
        effectParams[t] -= v;
    }

    public void AddCoef(ParamType t,float v)
    {
        coefParams[t] *= v;
    }
    public void RemoveCoef(ParamType t,float v)
    {
        coefParams[t] /= v;
    }

    public float this[ParamType t]
    {
        get { return Get(t); }
    }

    public void SetAbsolute(ParamType t,float v)
    {
        baseParams[t] = v;
    }
}


