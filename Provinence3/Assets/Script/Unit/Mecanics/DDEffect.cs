using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class DDEffect : TimeEffect
{
    public DDEffect(Unit targetUnit, float totalTime) 
        : base(targetUnit,totalTime)
    {
        EffectType = EffectType.doubleDamage;
        targetUnit.Parameters[ParamType.MPower] *= 2f;
        targetUnit.Parameters[ParamType.PPower] *= 2f;
        var effect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType.doubleDamage);
        effect.Init(targetUnit,endEffect);
        
    }

    protected override void OnTimer()
    {
        targetUnit.Parameters[ParamType.MPower] /= 2f;
        targetUnit.Parameters[ParamType.PPower] /= 2f;
        base.OnTimer();
    }
}

