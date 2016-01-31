using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class Talisman
{
    public TalismanItem sourseItem;
    public float currentEnergy;
    public Action<bool,float> OnReady;
    protected Hero hero;

    public Talisman(TalismanItem sourseItem,int countTalismans)
    {
        this.sourseItem = sourseItem;
        hero = MainController.Instance.level.MainHero;
        MainController.Instance.level.OnItemCollected += (id, f, delta) =>
        {
            if (id == ItemId.energy)
            {
                AddEnergy(delta / countTalismans);
            }
        };
    }

    public static Talisman Creat(TalismanItem sourseItem, int countTalismans)
    {
        Talisman talic = null;
        switch (sourseItem.TalismanType)
        {
            case TalismanType.speed:
                talic = new TalismanSpeed(sourseItem,countTalismans);
                break;
            case TalismanType.massPush:

                break;
            case TalismanType.firewave:
                talic = new TalismanFireWave(sourseItem, countTalismans);
                break;
            case TalismanType.massFreez:
                talic = new TalismanMassFreez(sourseItem, countTalismans);
                break;
            case TalismanType.heal:
                talic = new TalismanHeal(sourseItem, countTalismans);
                break;
            case TalismanType.doubleDamage:
                talic = new TalismanDoubleDamage(sourseItem, countTalismans);
                break;
            case TalismanType.chain:
                talic = new TalismanChain(sourseItem, countTalismans);
                break;
            case TalismanType.energyVamp:
                talic = new TalismanEnergyVamp(sourseItem, countTalismans);
                break;
            case TalismanType.bloodDamage:
                talic = new TalismanBloodDamage(sourseItem, countTalismans);
                break;
            case TalismanType.trapAOE:
                talic = new TalismanTrapAOE(sourseItem, countTalismans);
                break;
            case TalismanType.trapDamage:
                talic = new TalismanTrapDamage(sourseItem, countTalismans);
                break;
            case TalismanType.trapFreez:
                talic = new TalismanTrapFreez(sourseItem, countTalismans);
                break;
            case TalismanType.cleave:
                talic = new TalismanCleave(sourseItem, countTalismans);
                break;
        }
        return talic;
    }

    public virtual void Use()
    {
        Debug.Log("Use!!! " + sourseItem.TalismanType);
        AddEnergy(sourseItem.costShoot,true);
        DoCallback();
    }

    public void AddEnergy(float val, bool canBePositive = false)
    {
        if (canBePositive || val < 0)
        {
            currentEnergy = Mathf.Clamp(currentEnergy - val, 0, sourseItem.costShoot + 1);
            //Debug.Log("add energy " + currentEnergy + "/" + sourseItem.costShoot);
            DoCallback();
        }
    }

    private void DoCallback()
    {
        if (OnReady != null)
        {
            OnReady(CanUse(), currentEnergy/ (float)sourseItem.costShoot);
        }
    }

    public bool CanUse()
    {
        return currentEnergy >= sourseItem.costShoot;
    }
}

