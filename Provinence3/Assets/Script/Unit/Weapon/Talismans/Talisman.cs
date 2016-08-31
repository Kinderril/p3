﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class Talisman
{
    protected const float LVL_1_AV_MONSTER_HP = 100;
    protected const float LVL_10_AV_MONSTER_HP = 330;

    protected const string base_path = "prefabs/talisman/";
    public TalismanItem sourseItem;
    public float currentEnergy;
    public Action<bool,float,int> OnReady;
    protected Hero hero;
    private bool isUnderCooldown = false;
    private TimerManager.ITimer timer;
    private int currentCharges = 0;
    private float max;
    protected float power;
    private BaseEffectAbsorber CastEffect;

    public Talisman()
    {

    }

    public virtual string PowerInfo()
    {
        return power.ToString();
    }


    public virtual void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        this.sourseItem = sourseItem;
        max = sourseItem.costShoot * sourseItem.MaxCharges;
        if (level != null)
        {
            hero = level.MainHero;
            level.OnItemCollected += (id, f, delta) =>
            {
                if (id == ItemId.energy)
                {
                    AddEnergy(delta/countTalismans);
                }
            };
            var db = DataBaseController.Instance;
            var absorber = db.GetEffect(sourseItem.TalismanType);
            if (absorber != null)
            {
                CastEffect = DataBaseController.GetItem<BaseEffectAbsorber>(absorber);
                CastEffect.transform.SetParent(hero.transform,false);
            }
        }
        if (DebugController.Instance.ALL_TALISMAN_CHARGED)
        {
            currentEnergy = max;
        }
    }

    public void Dispose()
    {
        if (timer != null)
        {
            timer.Stop();
            timer = null;
        }
    }

    public static Talisman Creat(TalismanItem sourseItem, int countTalismans, Level level)
    {
        Talisman talic = null;
        switch (sourseItem.TalismanType)
        {
            case TalismanType.massPush:
                //
                break;
            case TalismanType.splitter:
                talic = new TalismanSplitter();
                break;
            case TalismanType.firewave:
                talic = new TalismanFireWave();
                break;
            case TalismanType.massFreez:
//                talic = new TalismanMassFreez(sourseItem, countTalismans);
                break;
            case TalismanType.heal:
                talic = new TalismanHeal();
                break;
            case TalismanType.doubleDamage:
                talic = new TalismanDoubleDamage();
                break;
            case TalismanType.chain:
                talic = new TalismanChain();
                break;
            case TalismanType.energyVamp:
//                talic = new TalismanEnergyVamp(sourseItem, countTalismans);
                break;
            case TalismanType.bloodDamage:
                talic = new TalismanBloodDamage();
                break;
            case TalismanType.trapAOE:
                talic = new TalismanTrapAOE();
                break;
            case TalismanType.trapDamage:
                talic = new TalismanTrapDamage();
                break;
            case TalismanType.trapFreez:
                talic = new TalismanTrapFreez();
                break;
            case TalismanType.megaArmor:
                talic = new TalismanArmor();
                break;

        }
        if (talic == null)
        {
            Debug.LogError("Talic creat error:" + sourseItem.TalismanType);
        }
        talic.Init(level,sourseItem,countTalismans);
        return talic;
    }

    public BaseMonster GetClosestMonster()
    {
        return Map.Instance.FindClosesEnemy(hero.transform.position);
    }

    public void UseIfCan()
    {
        Debug.Log("under !!! " + isUnderCooldown);
        if (CanUse())
            Use();
    }

    protected virtual void Use()
    {
        Debug.Log("Use!!! " + sourseItem.TalismanType);
        isUnderCooldown = true;
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromMilliseconds(1500));
        timer.OnTimer += OnTimerCome;
        AddEnergy(sourseItem.costShoot,true);
        if (CastEffect != null)
        {
            CastEffect.Play();
        }
        DoCallback();
    }

    private void OnTimerCome()
    {
        isUnderCooldown = false;
        DoCallback();
        timer = null;
    }

    public void AddEnergy(float val, bool canBePositive = false)
    {
        if (canBePositive || val < 0)
        {
            currentEnergy = Mathf.Clamp(currentEnergy - val, 0, max);
            DoCallback();
        }
    }

    private void DoCallback()
    {
        if (OnReady != null)
        {
            float percent = currentEnergy/(float) sourseItem.costShoot;
            currentCharges = (int)percent;
            if (percent > 1)
            {
                percent -= currentCharges;
            }
            
            OnReady(CanUse(), percent, currentCharges);
        }
    }

    public bool CanUse()
    {
        return currentEnergy >= sourseItem.costShoot && !isUnderCooldown;
    }
}

