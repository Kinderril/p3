using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum VisualEffectPosition
{
    core,
    weapon
}


public class VisualEffectBehaviour : PoolElement
{
    public VisualEffectPosition VisualEffectPosition;
    public EffectType EffectType;
    private IEndEffect toSubs;
    public BaseEffectAbsorber DispellEffect;
    public BaseEffectAbsorber SetEffectEffect;
    public BaseEffectAbsorber DistanceEFfect;
    private Unit lastUseUnit;

    public void Init(Unit unit,float timeWaitSec)
    {
        subInit(unit);
        base.Init();
        StartCoroutine(WaitFor(timeWaitSec));
    }
    public void Init(Unit unit,TimeEffect timeEffect)
    {
        Init(unit, timeEffect.endEffect);
    }

    public void SetColor(Color color)
    {
        SubSetColor(DispellEffect, color);
        SubSetColor(SetEffectEffect, color);
        SubSetColor(DistanceEFfect, color);
    }

    private void SubSetColor(BaseEffectAbsorber bea, Color color)
    {
        if (bea != null)
        {
            bea.SetColor(color);
        }
    }

    public void Init(Unit unit, IEndEffect toSubs)
    {
        this.toSubs = toSubs;
        this.toSubs.GetEndAction += EndEffect;
        subInit(unit);
        base.Init();
    }

    private void  subInit(Unit unit)
    {
        lastUseUnit = unit;
        transform.SetParent(unit.transform);
        transform.localPosition = Vector3.zero;
        switch (VisualEffectPosition)
        {
            case VisualEffectPosition.core:
                transform.SetParent(unit.transform, false);
                break;
            case VisualEffectPosition.weapon:
                transform.SetParent(unit.weaponsContainer, false);
                break;
        }
        lastUseUnit.OnDead += OnDead;
        StopAbsorber(DispellEffect);
        if (DistanceEFfect != null)
        {
            DistanceEFfect.Play();
        }
        if (SetEffectEffect != null)
        {
            SetEffectEffect.Play();
        }
    }

    private void OnDead(Unit obj)
    {
        EndEffect();
    }

    private void EndEffect()
    {
        Debug.Log("End use visual effet " + EffectType);
        if (toSubs != null)
        {
            toSubs.GetEndAction -= EndEffect;
            toSubs = null;
        }
        if (DispellEffect != null)
        {
            DispellEffect.Play();
        }
        StopAbsorber(DistanceEFfect);
        StopAbsorber(SetEffectEffect);
        lastUseUnit.OnDead -= OnDead;
        EndUse();
     }

    private void StopAbsorber(BaseEffectAbsorber psAbsorber)
    {
        if (psAbsorber != null)
        {
            psAbsorber.Stop();
        }
    }

    void OnDestroy()
    {
        if (lastUseUnit != null)
            lastUseUnit.OnDead -= OnDead;
    }

    private IEnumerator WaitFor(float timeSec)
    {
        yield return new WaitForSeconds(timeSec);
        EndEffect();
    }
}

