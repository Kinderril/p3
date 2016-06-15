﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum EffectType
{
    doubleDamage,
    parameter,
    heal,
    freez,
    fire,
    shield,
}

public class TimeEffect
{
    private float totalTime = 10;
    protected Unit targetUnit;
    protected TimerManager.ITimer timer;
    public IEndEffect endEffect;
    public EffectType EffectType;

    public static void Creat(Unit targetUnit, TimeEffect Effect)
    {

        TimeEffect oldEffect = null;
        if (targetUnit.efftcs.TryGetValue(Effect.EffectType, out oldEffect))
        {
            if (oldEffect != null)
                oldEffect.OnTimer();
        }
        Debug.Log("Effect setted " + Effect.EffectType);
        targetUnit.efftcs[Effect.EffectType] = Effect;
    }

    public static TimeEffect Creat(Unit targetUnit, EffectType EffectType,float power = 0, float totalTime = 10)
    {
        TimeEffect oldEffect = null;
        if (targetUnit.efftcs.TryGetValue(EffectType,out oldEffect))
        {
            if (oldEffect != null)
                oldEffect.OnTimer();
        }
        TimeEffect effect = null;
        Debug.Log("Effect setted " + EffectType);
        switch (EffectType)
        {
            case EffectType.doubleDamage:
                effect = new DDEffect(targetUnit, totalTime);
                break;
            case EffectType.freez:
                effect = new FreezEffet(targetUnit, totalTime);
                break;
            case EffectType.fire:
                effect = new FireEffect(targetUnit, totalTime, power);
                break;
            case EffectType.parameter:
                Debug.LogError("Dont creat this effect type bu this method");
                break;
        }
        targetUnit.efftcs[EffectType] = effect;
        return effect;
    }

    private void End()
    {
        targetUnit.efftcs[EffectType] = null;
        MainController.Instance.level.OnEndLevel -= OnEndLevel;
        timer.Stop();
        Debug.Log("Effect UnSET ");
        endEffect.Do();
    }

    public TimeEffect(Unit targetUnit, float totalTime)
    {
        this.totalTime = totalTime;
        this.targetUnit = targetUnit;
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromSeconds(totalTime));
        timer.OnTimer += OnTimer;
        MainController.Instance.level.OnEndLevel += OnEndLevel;
        targetUnit.OnDead += OnTargetDead;
        endEffect = new IEndEffect();
    }

    private void OnTargetDead(Unit obj)
    {
        End();
    }

    protected virtual void OnTimer()
    {
        End();
    }

    private void OnEndLevel()
    {
        End();
    }
}

