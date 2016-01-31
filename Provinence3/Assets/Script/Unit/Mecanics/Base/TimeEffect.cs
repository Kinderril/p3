using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum EffectType
{
    doubleDamage,
    slow,
    speed,
    heal,
    freez,
    fire,
    shield,
}

public class TimeEffect
{
    public float totalTime = 10;
    protected Unit targetUnit;
    protected TimerManager.ITimer timer;
//    public Action effectEnd;
    public IEndEffect endEffect;

    public static TimeEffect Creat(Unit targetUnit, EffectType EffectType,float power = 0)
    {
        Debug.Log("Effect setted " + EffectType);
        TimeEffect effect = null;
        switch (EffectType)
        {
            case EffectType.doubleDamage:
                effect = new DDEffect(targetUnit);
                break;
            case EffectType.slow:
//                targetUnit.Parameters.Parameters[ParamType.Speed] /= 3f;
                break;
            case EffectType.freez:

                break;
            case EffectType.speed:
                effect = new SpeedEffect(targetUnit);
                break;
            case EffectType.fire:
                effect = new FireEffect(targetUnit,power);
                break;
        }
        return effect;
    }

    public TimeEffect(Unit targetUnit)
    {
        this.targetUnit = targetUnit;
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromSeconds(totalTime));
        timer.OnTimer += OnTimer;
        MainController.Instance.level.OnEndLevel += OnEndLevel;
        targetUnit.OnDead += OnTargetDead;
        endEffect = new IEndEffect();
    }

    private void OnTargetDead(Unit obj)
    {
        OnEndLevel();
    }

    protected virtual void OnTimer()
    {
        OnEndLevel();
    }

    private void OnEndLevel()
    {
        MainController.Instance.level.OnEndLevel -= OnEndLevel;
        timer.Stop();
        Debug.Log("Effect UnSET ");
        endEffect.Do();
    }
}

