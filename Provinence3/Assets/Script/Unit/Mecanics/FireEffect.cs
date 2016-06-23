using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FireEffect : TimeEffect
{

    private float power;
    private bool shallStop = false;
    public FireEffect(Unit targetUnit,float power, float totalTime)
        : base(targetUnit, totalTime)
    {
        this.power = power;
        EffectType = EffectType.fire;
        var visualEffect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType);
        visualEffect.Init(targetUnit, endEffect);
        targetUnit.StartCoroutine(Burn());
    }

    private IEnumerator Burn()
    {
        
        yield return new WaitForSeconds(1);
        targetUnit.CurHp -= power;
        FlyNumberWIthDependence.Create(targetUnit.transform, "-" + power.ToString("0"));
        if (!shallStop && !targetUnit.IsDead)
        {
            targetUnit.StartCoroutine(Burn());
        }
    }

    protected override void OnTimer()
    {
        shallStop = true;
        base.OnTimer();
    }
}

