using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class Talisman
{
    protected const string base_path = "prefabs/talisman/";
    public TalismanItem sourseItem;
    public float currentEnergy;
    public Action<bool,float,int> OnReady;
    protected Hero hero;
    private bool isUnderCooldown = false;
    private TimerManager.ITimer timer;
    private int currentCharges = 0;

    public Talisman()
    {

    }

    public void Init(Level level, TalismanItem sourseItem, int countTalismans)
    {
        this.sourseItem = sourseItem;
        hero = level.MainHero;
        level.OnItemCollected += (id, f, delta) =>
        {
            if (id == ItemId.energy)
            {
                AddEnergy(delta / countTalismans);
            }
        };
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
            case TalismanType.speed:
                talic = new TalismanSpeed();
                break;
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
            case TalismanType.cleave:
                talic = new TalismanCleave();
                break;
                
        }
        talic.Init(level,sourseItem,countTalismans);
        return talic;
    }

    public BaseMonster GetClosestMonster()
    {
        return Map.Instance.FindClosesEnemy(hero.transform.position);
    }
    public virtual void Use()
    {
        Debug.Log("Use!!! " + sourseItem.TalismanType);
        isUnderCooldown = true;
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromMilliseconds(500));
        timer.OnTimer += OnTimerCome;
        AddEnergy(sourseItem.costShoot,true);
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
            currentEnergy = Mathf.Clamp(currentEnergy - val, 0, (sourseItem.costShoot )*sourseItem.MaxCharges );
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

