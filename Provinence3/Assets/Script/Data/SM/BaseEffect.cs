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
//    hpOfSelf,
    hpOfTarget,
//    heroMPower,
    charging,
    dependsOnDist
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
    private const float INSTANCE_COEF = 7f;
    private const float BASE_DURAION = 6f;
    private const float INSTANCE_COEF_SELF_INSTANT = 4.5f;

    public const int PATTACK_COEF = 8;
    public const int MATTACK_COEF = 9;
    public const int PDEF_COEF = 5;
    public const int MDEF_COEF = 7;
    public const int HP_COEF = 40;
    public const int HP_COEF_ADD = 100;
    public const int SPEED_COEF = 1;

    public const int MODIFY_BY_TYPE = 140;

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
                    return SubEffectData.Value;
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
        this.Duration = Duration;
        this.SubEffectData = SubEffectData;
        this.Spectial = spec;
    }

    public static BaseEffect Create(List<BaseEffect> effects, float resultPower, int level, EffectPositiveType positiveType, BaseSpell spell)
    {
        Console.WriteLine("start power:" + resultPower);
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
            default:
                throw new ArgumentOutOfRangeException("positiveType", positiveType, null);
        }
        
        var effect = BaseEffect.Create(specials, valuesType, paramsTypes, durations, resultPower, level, spell.TargetType);

        Console.WriteLine("end power:" + effect.CalcValue(spell));
        return effect;
    }

    public static BaseEffect CreateWithBase(BaseEffect oldData,float power, SpellTargetType targetType,int lvl)
    {
        ParamType t = ParamType.Heath;
        EffectValType vt = EffectValType.abs;
        if (oldData.SubEffectData != null)
        {
            t = oldData.SubEffectData.ParamType;
            vt = oldData.SubEffectData.EffectValType;
        }

        return new BaseEffect(oldData.Spectial,vt,t,oldData.Duration,power,lvl, targetType);
    }

    public BaseEffect(EffectSpectials special, EffectValType valuesType, ParamType paramsType,
        float duration, float resultPower, int level, SpellTargetType targetType)
    {
        Spectial = special;
        Duration = duration;
        switch (special)
        {
            case EffectSpectials.none:
            //            case EffectSpectials.hpOfSelf:
            case EffectSpectials.hpOfTarget:
            //            case EffectSpectials.heroMPower:
            case EffectSpectials.charging:
            case EffectSpectials.dependsOnDist:
                //            case EffectSpectials.bulletCount:
                SubEffectData = new SubEffectData(valuesType, paramsType, resultPower);
                if (targetType == SpellTargetType.Self && SubEffectData.ParamType != ParamType.Heath && Duration < 0.1f)
                {
                    resultPower /= INSTANCE_COEF_SELF_INSTANT;
                }
                break;
            case EffectSpectials.stun:
                Duration = PowerStun2Duration(resultPower, level);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (SubEffectData != null)
        {
            if (SubEffectData.ParamType != ParamType.Heath && Duration > 0.1f)
            {
                resultPower /= (Duration / BASE_DURAION);
            }
            resultPower = ModifFromType(resultPower, SubEffectData.ParamType);
            switch (SubEffectData.EffectValType)
            {
                case EffectValType.abs:
                    Value = resultPower;
                    break;
                case EffectValType.percent:
                    Value = Abs2Percent(level, resultPower, SubEffectData.ParamType, Duration <= 0f);
                    break;
            }

        }
    }

    public static BaseEffect Create(List<EffectSpectials> specials, List<EffectValType> valuesType, List<ParamType> paramsTypes, 
        List<float> durations, float resultPower, int level, SpellTargetType targetType)
    {
        //MainEffect = //TODO
        var spectial = specials.RandomElement();
        var valType = valuesType.RandomElement();
        var paramsType = paramsTypes.RandomElement();
        var dur2 = durations.RandomElement(2);
        var duration = SpellMerger.MergeVal(dur2[0], dur2[1], true);
        return new BaseEffect(spectial,valType,paramsType, duration,resultPower,level,targetType);
    }

    public float CalcValue(BaseSpell spell)
    {
        float val = Value;
        float durationCOef = 1f;
        //зависимоть процентного от непроцентного
        if (SubEffectData != null)
        {
            if (SubEffectData.EffectValType == EffectValType.percent)
            {
                val = Percent2Abs(spell.Level, Value, SubEffectData.ParamType, Duration <= 0f);
            }
            if (SubEffectData.ParamType != ParamType.Heath)
            {
                durationCOef = Duration/BASE_DURAION;
            }
        }
        else
        {
            switch (Spectial)
            {
                case EffectSpectials.stun:
                    val = DurationStun2Power(spell.Level);
                    break;
            }
        }
        float c;
        if (SubEffectData != null)
        {
            c = ModifByType(Mathf.Abs(val), SubEffectData.ParamType);
            if (durationCOef >= 0.1f)
            {
                c *= durationCOef;
            }
            if (spell.TargetType == SpellTargetType.Self && SubEffectData.ParamType != ParamType.Heath && Duration < 0.1f)
            {
                c *= INSTANCE_COEF_SELF_INSTANT;
            }
        }
        else
        {
            c= Mathf.Abs(val);
        }
        return c;
    }

    private float PByType(ParamType paramType)
    {
        float cur = 1f;
        switch (paramType)
        {
            case ParamType.PPower:
                cur = PATTACK_COEF;
                break;
            case ParamType.MPower:
                cur = MATTACK_COEF;
                break;
            case ParamType.PDef:
                cur = PDEF_COEF;
                break;
            case ParamType.MDef:
                cur = MDEF_COEF;
                break;
            case ParamType.Speed:
                cur = SPEED_COEF;
                break;
            case ParamType.Heath:
                cur = HP_COEF_ADD;
                break;
        }
        return cur;
    }

    private float ModifByType(float abs, ParamType paramType) //Обратная к ModifFromType
    {
        return abs* MODIFY_BY_TYPE / PByType(paramType);
    }

    private float ModifFromType(float val, ParamType paramType) //Обратная к ModifByType
    {
        return (PByType(paramType)*val)/ MODIFY_BY_TYPE;
    }


    private float Abs2Percent(int level, float v, ParamType paramType, bool isIntance)
    {
        var m = MiddleParamByLevel(level, paramType);
        var r = 100 * v / m;
        if (isIntance && paramType != ParamType.Heath)
        {
            r /= INSTANCE_COEF;
        }
        return r;
    }

    private float Percent2Abs(int level, float v,ParamType paramType,bool isIntance)
    {
        var m = MiddleParamByLevel(level, paramType);
        var res = m * v / 100f;
        if (isIntance && paramType != ParamType.Heath)
        {
            res *= INSTANCE_COEF;
        }
        return res;
    }

    public float MiddleParamByLevel(int lvl, ParamType paramType)
    {
        switch (paramType)
        {
            case ParamType.PPower:
                return 30 + lvl * PATTACK_COEF;
            case ParamType.MPower:
                return 20 + lvl * MATTACK_COEF;
            case ParamType.PDef:
                return 9 + lvl * PDEF_COEF;
            case ParamType.MDef:
                return 4 + lvl * MDEF_COEF;
            case ParamType.Heath:
                return 500 + lvl * HP_COEF;
            case ParamType.Speed:
                return 400 + lvl * SPEED_COEF;
        }
        return 300;
    }

    private float DurationStun2Power(int spellLevel)
    {
        return Duration*(40 + 20*spellLevel);
    }

    private float PowerStun2Duration(float power, int spellLevel)
    {
        return power/(40 + 20*spellLevel);
    }

    public string Desc(BaseSpell spell, EffectPositiveType positiveType)
    {
        var evt = "";
//        var powerStr = " power:" + CalcValue(spell).ToString("0.0") + " ";
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
            dur = " instantly.";
        }
        else
        {
            dur = " for " + Duration.ToString("0.0") + " sec.";
        }

        switch (Spectial)
        {
            case EffectSpectials.stun:
                spc += "Do stun effect for " + Duration.ToString("0.00");
                break;
            default:
                var val = Mathf.Abs(SubEffectData.Value).ToString("0.0");
                if (SubEffectData.ParamType == ParamType.Heath)
                {
                    if (positiveType == EffectPositiveType.Positive)
                    {
                        spc = "Heal on " + val;
                    }
                    else
                    {
                        spc = "Damage on " + val;
                    }
                }
                else
                {
                    if (positiveType == EffectPositiveType.Positive)
                    {
                        spc = "Increase "+ NameParam(SubEffectData.ParamType) + " on " + val;
                    }
                    else
                    {
                        spc = "Decrease "+ NameParam(SubEffectData.ParamType) + " on " + val;
                    }
                }

                spc += evt + dur;
                break;
        }
        switch (Spectial)
        {
//            case EffectSpectials.hpOfSelf:
//                spc += " Depence on heath of hero.";
//                break;
            case EffectSpectials.hpOfTarget:
                spc += " Depence on heath of target.";
                break;
//            case EffectSpectials.heroMPower:
//                spc += " Depence on magic power of hero.";
//                break;
            case EffectSpectials.charging:
                spc += " Charging for more damage.";
                break;
            case EffectSpectials.dependsOnDist:
                spc += " Power dependce on distance.";
                break;
                //            case EffectSpectials.bulletCount:
                //                spc += " depence on bullets launched";
                //                break;
        }
        return spc;
    }

    private string NameParam(ParamType p)
    {
        return p.ToString();
    }

    public string DescFull(BaseSpell spell, EffectPositiveType type)
    {
        var evt = "";
        var powerStr = " power:" + CalcValue(spell).ToString("0.0") + " ";
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
            dur = " instant ";
        }
        else
        {
            dur = " for " + Duration.ToString("0.00");
        }

        switch (Spectial)
        {
            case EffectSpectials.stun:
                spc += "Do stun effect for " + Duration.ToString("0.00");
                break;
            default:
                spc = "Affect on " + SubEffectData.ParamType + "  " + SubEffectData.Value.ToString("0.0") + "  " + evt + " " + dur;
                break;
        }
        switch (Spectial)
        {
//            case EffectSpectials.hpOfSelf:
//                spc += " depence on heath of hero";
//                break;
            case EffectSpectials.hpOfTarget:
                spc += " depence on heath of target";
                break;
//            case EffectSpectials.heroMPower:
//                spc += " depence on magic power of hero";
//                break;
            case EffectSpectials.charging:
                spc += " charging for more damage.";
                break;
            case EffectSpectials.dependsOnDist:
                spc += " dependce on distance.";
                break;
                //            case EffectSpectials.bulletCount:
                //                spc += " depence on bullets launched";
                //                break;
        }
        return spc + "." + powerStr;
    }


    public string Save()
    {
        var result = Id.ToString() + SMUtils.DELEM + Duration + SMUtils.DELEM + Spectial.ToString() + SMUtils.DELEM + EffectVisualId;
        string substr = "";
        if (SubEffectData != null)
        {
            substr = SMUtils.DELEM + SubEffectData.EffectValType.ToString() + SMUtils.DELEM + SubEffectData.ParamType.ToString() + SMUtils.DELEM + SubEffectData.Value;
        }
        return result + substr;
    }

    public static BaseEffect Load(string info)
    {
        var ss = info.Split(SMUtils.DELEM);
        int Id = Convert.ToInt32(ss[0]);
        float Duration = Convert.ToSingle(ss[1]);
        EffectSpectials special = (EffectSpectials) Enum.Parse(typeof (EffectSpectials), ss[2]);
        int visualId = Convert.ToInt32(ss[3]);
        SubEffectData subEffect = null;
        if (ss.Length > 5)
        {
            EffectValType efd = (EffectValType) Enum.Parse(typeof (EffectValType), ss[4]);
            ParamType paramType = (ParamType) Enum.Parse(typeof (ParamType), ss[5]);
            float val = Convert.ToSingle(ss[6]);
            subEffect = new SubEffectData(efd, paramType, val);
        }
        BaseEffect effect = new BaseEffect(Duration, subEffect, special);
        effect.Id = Id;
        effect.EffectVisualId = visualId;
        return effect;
    }
}

