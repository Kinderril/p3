using System;
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
        CheckOldEffect(targetUnit, Effect);
        targetUnit.efftcs.Add(Effect);
    }

    public static TimeEffect Creat(Unit targetUnit, EffectType EffectType,float power = 0, float totalTime = 10)
    {
        TimeEffect effect = null;
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
        CheckOldEffect(targetUnit, effect);
        targetUnit.efftcs.Add(effect);
            return effect;
    }
    
    private static void CheckOldEffect(Unit targetUnit, TimeEffect nEffect)
    {
        TimeEffect oldEffect = targetUnit.efftcs.FirstOrDefault(x => x.EffectType == nEffect.EffectType);
        if (oldEffect != null)
        {
            var oldParamEffect = oldEffect as ParameterEffect;
            var nParamEffect = nEffect as ParameterEffect;
            if (oldParamEffect != null && nParamEffect != null)
            {
                if (oldParamEffect.Type == nParamEffect.Type)
                {
                    oldEffect.OnTimer();
                }
            }
            else
            {
                oldEffect.OnTimer();
            }
        }
        Debug.Log("Effect setted:" + nEffect);
    }
    private void End()
    {
        timer.Stop();
        targetUnit.efftcs.Remove(this);
        targetUnit.OnDead -= OnTargetDead;
        MainController.Instance.level.OnEndLevel -= OnEndLevel;
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

