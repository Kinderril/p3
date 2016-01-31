using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DDEffect : TimeEffect
{
    public DDEffect(Unit targetUnit) 
        : base(targetUnit)
    {
        targetUnit.Parameters.Parameters[ParamType.MPower] *= 2f;
        targetUnit.Parameters.Parameters[ParamType.PPower] *= 2f;
        var effect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType.doubleDamage);
        effect.Init(targetUnit,endEffect);
        
    }

    protected override void OnTimer()
    {
        targetUnit.Parameters.Parameters[ParamType.MPower] /= 2f;
        targetUnit.Parameters.Parameters[ParamType.PPower] /= 2f;
        base.OnTimer();
    }
}

