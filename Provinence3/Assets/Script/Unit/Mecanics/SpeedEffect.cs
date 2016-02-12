using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SpeedEffect : TimeEffect
{
    private bool plus;
    public SpeedEffect(Unit targetUnit,float  totalTime,bool plus = true) 
        : base(targetUnit, totalTime)
    {
        EffectType = EffectType.slow;
        this.plus = plus;
        if (plus)
        {
            targetUnit.Parameters.Parameters[ParamType.Speed] *= 2f;
        }
        else
        {
            targetUnit.Parameters.Parameters[ParamType.Speed] /= 2f;
        }
        var visualEffect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType.speed);
        visualEffect.Init(targetUnit,endEffect);
    }

    protected override void OnTimer()
    {
        if (plus)
        {
            targetUnit.Parameters.Parameters[ParamType.Speed] /= 2f;
        }
        else
        {
            targetUnit.Parameters.Parameters[ParamType.Speed] *= 2f;
        }
        base.OnTimer();
    }
}

