using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SpeedEffect : TimeEffect
{

    public SpeedEffect(Unit targetUnit) 
        : base(targetUnit)
    {
        targetUnit.Parameters.Parameters[ParamType.Speed] *= 2f;
        var effect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType.speed);
        effect.Init(targetUnit,endEffect);
    }

    protected override void OnTimer()
    {
        targetUnit.Parameters.Parameters[ParamType.Speed] /= 2f;
        base.OnTimer();
    }
}

