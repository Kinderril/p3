using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum BonusElementMapType
{
    heal,
    shield,
    speed,
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
            case BonusElementMapType.speed:
                var e = new ParameterEffect(hero,10,ParamType.Speed, 2f);
                TimeEffect.Creat(hero, e);
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

