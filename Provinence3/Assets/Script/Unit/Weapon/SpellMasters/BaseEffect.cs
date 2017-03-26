using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


public enum EffectValType
{
    abs,
    percent,
}

public enum EffectSpectials
{
    none,
    stun,
    hpOfSelf,
    hpOfTarget,
    bulletCount,
}

public class SubEffectData
{
    public ParamType ParamType;
    public EffectValType EffectValType;
    public float Value;
    
    public SubEffectData(EffectValType effectValType, ParamType paramType, float resultPower)
    {
        EffectValType = effectValType;
        ParamType = paramType;
        this.Value = resultPower;
    }
}

public class BaseEffect
{
    public int Id;
    public SubEffectData SubEffectData;
    public float Duration;
    public EffectSpectials Spectial;
    public int EffectVisualId;

    public float Value
    {
        get
        {
            switch (Spectial)
            {
                case EffectSpectials.none:
                    return SubEffectData.Value;
                case EffectSpectials.stun:
                    return Duration;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private set
        {
            if (SubEffectData != null)
            {
                SubEffectData.Value = value;
            }
        }
    }

    public BaseEffect(float Duration, SubEffectData SubEffectData, EffectSpectials spec)
    {
        //TODO this base constructor
        this.Duration = Duration;
        this.SubEffectData = SubEffectData;
        this.Spectial = spec;
    }

    public static BaseEffect Create(List<BaseEffect> effects, float resultPower, int level, EffectPositiveType positiveType)
    {
        List<EffectSpectials> specials = new List<EffectSpectials>();
        List< EffectValType > valuesType = new List<EffectValType>();
        List<ParamType> paramsTypes = new List<ParamType>();
        List< float > durations = new List<float>();
        foreach (var baseEffect in effects)
        {
            specials.Add(baseEffect.Spectial);
            if (baseEffect.SubEffectData != null)
            {
                valuesType.Add(baseEffect.SubEffectData.EffectValType);
                paramsTypes.Add(baseEffect.SubEffectData.ParamType);
            }
            durations.Add(baseEffect.Duration);
        }
        switch (positiveType)
        {
            case EffectPositiveType.Positive:
                resultPower = Mathf.Abs(resultPower);
                break;
            case EffectPositiveType.Negative:
                resultPower = -Mathf.Abs(resultPower);
                break;
            case EffectPositiveType.Both:
                resultPower =  UnityEngine.Random.value < 0.5f ? Mathf.Abs(resultPower) : -Mathf.Abs(resultPower);
                break;
            default:
                throw new ArgumentOutOfRangeException("positiveType", positiveType, null);
        }
        
        return new BaseEffect(specials, valuesType, paramsTypes, durations, resultPower, level);
    }

    private BaseEffect(List<EffectSpectials> specials, List<EffectValType> valuesType, List<ParamType> paramsTypes, List<float> durations, float resultPower, int level)
    {
        //MainEffect = //TODO
        Spectial = specials.RandomElement();
        switch (Spectial)
        {
            case EffectSpectials.none:
            case EffectSpectials.hpOfSelf:
            case EffectSpectials.hpOfTarget:
            case EffectSpectials.bulletCount:
                SubEffectData = new SubEffectData(valuesType.RandomElement(), paramsTypes.RandomElement(), resultPower);
                var dur2 = durations.RandomElement(2);
                Duration = SpellMerger.MergeVal(dur2[0], dur2[1], true);
                break;
            case EffectSpectials.stun:
                Duration = PowerStun2Duration(resultPower, level);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (SubEffectData != null)
        {
            switch (SubEffectData.EffectValType)
            {
                case EffectValType.abs:
                    Value = resultPower;
                    break;
                case EffectValType.percent:
                    Value = Abs2Percent(level, resultPower);
                    break;
            }
        }
    }

    public float CalcValue(int spellLevel)
    {
        float val = Value;
        //зависимоть процентного от непроцентного
        if (SubEffectData != null)
        {
            if (SubEffectData.EffectValType == EffectValType.percent)
            {
                val = Percent2Abs(spellLevel, Value);
            }
        }
        else
        {
            switch (Spectial)
            {
                case EffectSpectials.stun:
                    val = DurationStun2Power(spellLevel);
                    break;
            }
        }
        return val;
    }

    private float Abs2Percent(int level,float res)
    {
        var m = MiddleHPByLevel(level);
       return res/(m*0.7f);
    }

    private float Percent2Abs(int level, float v)
    {
        var m = MiddleHPByLevel(level);
        var res = m * 0.7f * v;
        return res;
    }

    public float MiddleHPByLevel(int lvl)
    {
        return 100;
    }

    private float DurationStun2Power(int spellLevel)
    {
        return Duration*(40 + 20*spellLevel);
    }

    private float PowerStun2Duration(float power, int spellLevel)
    {
        return power/(40 + 20*spellLevel);
    }

    public string Desc()
    {
        var evt = "";
        switch (SubEffectData.EffectValType)
        {
            case EffectValType.percent:
                evt = "%";
                break;
        }
        var spc = "";
        var dur = "";
        if (Duration == 0f)
        {
            dur = "instant";
        }
        else
        {
            dur = "for " + Duration.ToString("0.00");
        }

        switch (Spectial)
        {
            case EffectSpectials.stun:
                spc += "Do stun effect for " + Duration.ToString("0.00");
                break;
            case EffectSpectials.none:
                spc = "Affect on " + SubEffectData.ParamType + evt + " " + dur;
                break;
        }
        switch (Spectial)
        {
            case EffectSpectials.hpOfSelf:
                spc += " depence on heath of hero";
                break;
            case EffectSpectials.hpOfTarget:
                spc += " depence on heath of target";
                break;
            case EffectSpectials.bulletCount:
                spc += " depence on bullets launched";
                break;
        }
        return spc + ".";
    }
}

