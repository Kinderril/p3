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
    private float coef;
    public ParameterEffect(Unit targetUnit, float totalTime, ParamType type, float coef , bool plus = true)
        : base(targetUnit, totalTime)
    {
        this.Type = type;
        EffectType = EffectType.parameter;
        this.coef = coef;
        this.plus = plus;
        if (plus)
        {
            targetUnit.Parameters.Parameters[type] *= coef;
        }
        else
        {
            targetUnit.Parameters.Parameters[type] /= coef;
        }
        CheckOnSpeed();
        var visualEffect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType);
        visualEffect.Init(targetUnit, endEffect);
        var paramColor = DataBaseController.Instance.GetColor(type);
        visualEffect.SetColor(paramColor);
        var oldP = visualEffect.transform.localPosition;
        visualEffect.transform.localPosition = new Vector3(oldP.x,oldP.y,oldP.z + Random.Range(0,3));
    }

    private void CheckOnSpeed()
    {
        if (Type == ParamType.Speed)
        {
            targetUnit.Control.SetSpeed(targetUnit.Parameters.Parameters[Type]);
        }
    }

    protected override void OnTimer()
    {
        if (plus)
        {
            targetUnit.Parameters.Parameters[Type] /= coef;
        }
        else
        {
            targetUnit.Parameters.Parameters[Type] *= coef;
        }
        CheckOnSpeed();
        base.OnTimer();
    }
}

