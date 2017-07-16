using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class TriggerInGame
{
    private const float SQR_DIST_DEATH = 5*5;
    private BaseSpell spellData;
    private int chargesRemain = 0;
    private Unit Owner;
    private BaseTrigger triggerData;
    private SpellInGame sourseItem;

    public TriggerInGame(SpellInGame sourseItem,Unit owner)
    {
        this.sourseItem = sourseItem;
        Owner = owner;
        spellData = sourseItem.sourseItem.SpellData;
        triggerData = sourseItem.sourseItem.SpellData.BaseTrigger;
    }

    private void OnHeal()
    {
        Activate();
    }

    private void OnGetEffect()
    {
        Activate();
    }

    private void OnItemCollected(ItemId arg1, float arg2, float arg3)
    {
        if (arg1 == ItemId.money)
        {
            Activate();
        }
    }

    private void OnEnemyDead(Unit obj)
    {
        var dir = obj.transform.position - Owner.transform.position;
        if (dir.sqrMagnitude < SQR_DIST_DEATH)
        {
            Activate();
        }

    }

    private void OnCastSpell(SpellInGame obj)
    {
        Activate();
    }

    private void OnGetHit(float arg1, float arg2, float arg3)
    {
        Activate();
    }

    private void OnUnitAttack(Vector3 obj)
    {
        if (Owner.curWeapon.DamageType == WeaponType.physics)
        {
            if (triggerData.TriggerType == SpellTriggerType.shoot)
            {
                Activate();
            }
        }
        else
        {
            if (triggerData.TriggerType == SpellTriggerType.shootMagic)
            {
                Activate();
            }
        }
    }

    public void Activate()
    {
        sourseItem.ActivateByTrigger();
        chargesRemain--;
        
        if (chargesRemain <= 0)
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        switch (triggerData.TriggerType)
        {
            case SpellTriggerType.shoot:
                Owner.OnUnitAttack -= OnUnitAttack;
                break;
            case SpellTriggerType.shootMagic:
                Owner.OnUnitAttack -= OnUnitAttack;
                break;
            case SpellTriggerType.getDamage:
                Owner.OnGetHit -= OnGetHit;
                break;
            case SpellTriggerType.cast:
                Owner.OnCastSpell -= OnCastSpell;
                break;
            case SpellTriggerType.deathNear:
                Map.Instance.OnEnemyDeadCallback -= OnEnemyDead;
                break;
            case SpellTriggerType.getGold:
                MainController.Instance.level.OnItemCollected -= OnItemCollected;
                break;
            //                case SpellTriggerType.questAction:
            //                    break;
            case SpellTriggerType.getHeal:
                Owner.OnHeal -= OnHeal;
                break;
            case SpellTriggerType.getEffect:
                Owner.OnGetEffect -= OnGetEffect;
                break;
            default:
                Debug.LogError("need to realize this type:" + triggerData.TriggerType);
                break;
        }
    }

    public void Use()
    {
        if (chargesRemain <= 0)
        {
            switch (triggerData.TriggerType)
            {
                case SpellTriggerType.shoot:
                    Owner.OnUnitAttack += OnUnitAttack;
                    break;
                case SpellTriggerType.shootMagic:
                    Owner.OnUnitAttack += OnUnitAttack;
                    break;
                case SpellTriggerType.getDamage:
                    Owner.OnGetHit += OnGetHit;
                    break;
                case SpellTriggerType.cast:
                    Owner.OnCastSpell += OnCastSpell;
                    break;
                case SpellTriggerType.deathNear:
                    Map.Instance.OnEnemyDeadCallback += OnEnemyDead;
                    break;
                case SpellTriggerType.getGold:
                    MainController.Instance.level.OnItemCollected += OnItemCollected;
                    break;
                case SpellTriggerType.getHeal:
                    Owner.OnHeal += OnHeal;
                    break;
                case SpellTriggerType.getEffect:
                    Owner.OnGetEffect += OnGetEffect;
                    break;
                default:
                    Debug.LogError("need to realize this type:" + triggerData.TriggerType);
                    break;
            }
        }
        chargesRemain = spellData.BaseTrigger.ShootCount; //TODO
    }
}

