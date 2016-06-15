using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ParameterEffect : TimeEffect
{
    private bool plus;
    private ParamType type;
    private float coef;
    public ParameterEffect(Unit targetUnit, float totalTime, ParamType type, float coef , bool plus = true)
        : base(targetUnit, totalTime)
    {
        this.type = type;
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
        var visualEffect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType);
        visualEffect.Init(targetUnit, endEffect);
        var paramColor = DataBaseController.Instance.GetColor(type);
        visualEffect.SetColor(paramColor);
    }

    protected override void OnTimer()
    {
        if (plus)
        {
            targetUnit.Parameters.Parameters[type] /= coef;
        }
        else
        {
            targetUnit.Parameters.Parameters[type] *= coef;
        }
        base.OnTimer();
    }
}

