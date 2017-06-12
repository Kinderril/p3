using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


public class ParameterEffect : TimeEffect
{
    private bool plus;
    public ParamType Type;
    private EffectValType typeVal;
    private float _value;
    public ParameterEffect(Unit targetUnit, float totalTime, ParamType type, float value , bool plus , EffectValType typeVal)
        : base(targetUnit, totalTime)
    {
        this.Type = type;
        this.typeVal = typeVal;
        EffectType = EffectType.parameter;
        this._value = value;
        this.plus = plus;
        switch (typeVal)
        {
            case EffectValType.abs:
                if (plus)
                {
                    targetUnit.Parameters.Add(Type, value);
                }
                else
                {
                    targetUnit.Parameters.Remove(Type, value);
                }
                break;
            case EffectValType.percent:
                if (plus)
                {
                    targetUnit.Parameters.AddCoef(Type, value);
                }
                else
                {
                    targetUnit.Parameters.RemoveCoef(Type, value);
                }
                break;
        }
        CheckOnSpeed();
        var visualEffect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType);
        visualEffect.Init(targetUnit, endEffect);
        var paramColor = DataBaseController.Instance.GetColor(type);
        visualEffect.SetColor(paramColor);
        var oldP = visualEffect.transform.localPosition;
        visualEffect.transform.localPosition = new Vector3(oldP.x,oldP.y + Random.Range(0, 2), oldP.z );
    }

    private void CheckOnSpeed()
    {
        if (Type == ParamType.Speed)
        {
            targetUnit.Control.SetSpeed(targetUnit.Parameters[Type]);
        }
    }

    protected override void OnTimer()
    {
        EndEffect();
        CheckOnSpeed();
        base.OnTimer();
    }

    private void EndEffect()
    {
        switch (typeVal)
        {
            case EffectValType.abs:
                if (plus)
                {
                    targetUnit.Parameters.Remove(Type, _value);
                }
                else
                {
                    targetUnit.Parameters.Add(Type, _value);
                }
                break;
            case EffectValType.percent:
                if (plus)
                {
                    targetUnit.Parameters.RemoveCoef(Type, _value);
                }
                else
                {
                    targetUnit.Parameters.AddCoef(Type, _value);
                }
                break;
        }
    }
}

