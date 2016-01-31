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
    public PSAbsorber effect;
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
                if (effect != null)
                    effect.Play();
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
                hero.GetHeal((hero.Parameters.MaxHp - hero.CurHp)/2f);
                break;
            case BonusElementMapType.shield:
                hero.Shield = hero.Parameters.MaxHp/3f;
                break;
            case BonusElementMapType.speed:
                var speedEffect = new SpeedEffect(hero);
                break;
            case BonusElementMapType.killAll:
                var enemies = Map.Instance.GetEnimiesInRadius(30);
                foreach (var baseMonster in enemies)
                {
                    baseMonster.CurHp -= 90000;
                }
                break;
        }
    }
}

