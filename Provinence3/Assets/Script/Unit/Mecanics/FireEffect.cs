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
    public FireEffect(Unit targetUnit,float power)
        : base(targetUnit)
    {
        targetUnit.StartCoroutine(Burn());
    }

    private IEnumerator Burn()
    {
        yield return new WaitForSeconds(1);
        targetUnit.CurHp -= power;
        if (!shallStop)
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

