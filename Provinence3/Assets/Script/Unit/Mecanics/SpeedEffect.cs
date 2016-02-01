using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SpeedEffect : TimeEffect
{
    private bool plus;
    public SpeedEffect(Unit targetUnit,bool plus = true) 
        : base(targetUnit)
    {
        this.plus = plus;
        if (plus)
        {
            targetUnit.Parameters.Parameters[ParamType.Speed] *= 2f;
        }
        else
        {
            targetUnit.Parameters.Parameters[ParamType.Speed] /= 2f;
        }
        var effect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType.speed);
        effect.Init(targetUnit,endEffect);
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

