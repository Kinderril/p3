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
}
public class BaseBonusMapElement : MonoBehaviour
{
    public BonusElementMapType bonusElementMapType;
    private bool isActive = true;
    public PSAbsorber effectOnGet;
    public GameObject ActivePart;

    public void Init()
    {
        Utils.GroundTransform(transform);
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
            case BonusElementMapType.heal:
                hero.GetHeal((hero.Parameters.MaxHp)/2f);
                break;
            case BonusElementMapType.shield:
                hero.Shield = hero.Parameters.MaxHp/3f;
                break;
            case BonusElementMapType.speed:
                var e = new ParameterEffect(hero,10,ParamType.PPower, 1.5f);
                TimeEffect.Creat(hero, e);
                break;
            case BonusElementMapType.killAll:
                var enemies = Map.Instance.GetEnimiesInRadius(36);
                foreach (var baseMonster in enemies)
                {
                    Debug.Log(" " + baseMonster.mainHeroDist);
                    baseMonster.CurHp -= 90000;
                }
                break;
        }
    }
}

