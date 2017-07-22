﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BonusElementMapType
{
    heal,
    shield,
    armor,
    killAll,
    energy,
}
public class BaseBonusMapElement : MonoBehaviour
{
    public BonusElementMapType bonusElementMapType;
    private bool isActive = true;
    public BaseEffectAbsorber effectOnGet;
    public GameObject ActivePart;

    public void Init()
    {
        Utils.GroundTransform(transform);
        effectOnGet.Stop();
    }
    void OnTriggerEnter(Collider other)
    {
        if (isActive)
        {
            var unit = other.GetComponent<Hero>();
            if (unit != null)
            {
                PlayOpen(unit);
                if (effectOnGet != null)
                    effectOnGet.Play();
            }
        }
    }

    private void PlayOpen(Hero hero)
    {
        isActive = false;
        ActivePart.gameObject.SetActive(isActive);
        switch (bonusElementMapType)
        {
            case BonusElementMapType.energy:
                MainController.Instance.level.AddItem(ItemId.energy, -25);
                break;
            case BonusElementMapType.heal:
                hero.GetHeal((hero.Parameters.MaxHp)/2f);
                break;
            case BonusElementMapType.shield:
                hero.Shield = hero.Parameters.MaxHp/3f;
                break;
            case BonusElementMapType.armor:
                var e = new ParameterEffect(hero,20,ParamType.PDef, 1f,true,EffectValType.percent);
                TimeEffect.Execute(hero, e);
                var e2 = new ParameterEffect(hero,20,ParamType.MDef, 1f,true,EffectValType.percent);
                TimeEffect.Execute(hero, e2);
                break;
            case BonusElementMapType.killAll:
                var enemies = Map.Instance.GetEnimiesInRadius(36);
                foreach (var baseMonster in enemies)
                {
                    if (baseMonster is BossUnit)
                    {
                        continue;
                    }
                    Debug.Log(" " + baseMonster.mainHeroDist);
                    baseMonster.SetHp(-1);
                }
                break;
        }
    }
}

