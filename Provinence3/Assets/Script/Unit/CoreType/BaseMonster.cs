using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct DropItem
{
    public float chance0_1;
    public CraftItemType type;
    public DropItem(float chance, CraftItemType type)
    {
        this.chance0_1 = chance;
        this.type = type;
    }

}

public enum AIStatus
{
    attack,
    returnHome,
    walk,
    secondaryAction,
    disable,
}

public class BaseMonster : Unit
{
    private const float isHomeDist = 2;
    public const float AI_DIST = 170;
    private const float runAwayDist = 110;

    public float attackDist = 45;
    public float mainHeroDist = 0;
    public Vector3 bornPosition;
    public AIStatus aiStatus;
    private Hero mainHero;
    private int energyadd;
    private bool isHome = true;
    private BaseAction attackBehaviour;
    private bool isDisabled = false;
    public bool haveAction;
    public List<DropItem> dropItems; 
    public FlashController FlashController;
    private bool overcharged = false;

    public float moneyCoef = 1f;
    public float energyCoef = 1f;


    public bool IsDisabled
    {
        get { return isDisabled; }
    }


    public void Init(Hero hero)
    {
        mainHero = hero;
        Init();
    }

    public override void Init()
    {
        base.Init();
        bornPosition = transform.position;
        Utils.GroundTransform(transform, 999f);
        energyadd = (int)(Energy.CREEP_ENERGY_AV*energyCoef);
        aiStatus = AIStatus.disable;
    }

    public void Overcharg()
    {
        Dictionary<ParamType, float> tmpDictionary = new Dictionary<ParamType, float>();
        foreach (var unitParameter in Parameters)
        {
            float c = 1f;
            switch (unitParameter.Key)
            {
                case ParamType.Speed:
                    c = 1.15f;
                    break;
                case ParamType.MPower:
                case ParamType.PPower:
                case ParamType.PDef:
                case ParamType.MDef:
                    c = 1.3f;
                    break;
                case ParamType.Heath:
                    c = 2.3f;
                    curHp = unitParameter.Value*c;
                    break;
            }
            var upg = unitParameter.Value*c;
            tmpDictionary[unitParameter.Key] = upg;
        }
        foreach (var f in tmpDictionary)
        {
            Parameters[f.Key] = f.Value;
        }
        transform.localScale = Vector3.one*1.5f;
        overcharged = true;
        moneyCoef *= 4f;
        energyCoef *= 5f;
    }

    protected override void Death()
    {
        Control.Dead();
        MainController.Instance.level.AddItem(ItemId.energy, -energyadd);
        
        DropItems();
        DropHeal();
        DropMoney();

        base.Death();
        Action = null;
    }

    private void DropHeal()
    {
        var isDrop = UnityEngine.Random.Range(0, 100) < 12;
#if UNITY_EDITOR
        if (DebugController.Instance.ALL_TIME_DROP)
        {
            isDrop = true;
        }
#endif
        if (isDrop)
        {
            var HealMapItem = DataBaseController.GetItem<HealMapItem>(DataBaseController.Instance.HealMapItemPrefab, transform.position);
            MapItemInit(HealMapItem);
        }
    }

    private void DropItems()
    {

        foreach (var dropItem in dropItems)
        {
            var chance = UnityEngine.Random.Range(0f, 1f);
            if (overcharged)
            {
                chance /= 3;
            }
            var isDrop = chance < dropItem.chance0_1;
#if UNITY_EDITOR
            if (DebugController.Instance.ALL_TIME_DROP)
            {
                isDrop = true;
            }
#endif
            if (isDrop)
            {
                DropItem(dropItem.type);
            }
        }
    }

    private void DropItem(CraftItemType t)
    {
        int count = 1;
        if (overcharged)
        {
            count = 4;
        }
        for (int i = 0; i < count; i++)
        {
            var itemMapItem = DataBaseController.GetItem<ItemMapItem>(DataBaseController.Instance.ItemMapItemPrefab, transform.position);
            itemMapItem.Init(t);
            MapItemInit(itemMapItem);
        }
    }

    private void MapItemInit(BaseMapItem item)
    {
        item.transform.SetParent(Map.Instance.miscContainer, true);
        item.StartFly(transform);
    }
    
    private void DropMoney()
    {
        var goldMapItem = DataBaseController.GetItem<GoldMapItem>(DataBaseController.Instance.GoldMapItemPrefab, transform.position);
        goldMapItem.Init(ItemId.money, Formuls.GoldInMonster(Parameters.Level,moneyCoef));
        MapItemInit(goldMapItem);
    }

    public override void GetHit(Bullet bullet)
    {
        var hp = CurHp;
        base.GetHit(bullet);
        hp -= CurHp;
        if (hp > 0)
        {
            FlyNumberWIthDependence.Create(transform, "-" + hp.ToString("0"));
            if (FlashController != null)
                FlashController.Play();
            GlobalEventManager.Instance.MonsterGetHit(this);
        }
        if (attackBehaviour == null)
        {
            StartAttack(true);
        }

    }
    
    public void CheckDistance()
    {
        if (mainHero == null)
            return;
        
        var isInAI = mainHeroDist < AI_DIST;
        bool isTargetClose = (mainHeroDist < attackDist);
        
        switch (aiStatus)
        {
            case AIStatus.attack:
                if ((mainHeroDist > runAwayDist))
                {
                    ReturnHome();
                }
                break;
            case AIStatus.returnHome:
                if (isTargetClose)
                {
                    StartAttack(false);
                }
                break;
            case AIStatus.walk:
                if (!isInAI)
                {
                    Disable();
                }
                else if (isTargetClose)
                {
                    StartAttack(false);
                }
                break;
            case AIStatus.disable:
                if (isTargetClose)
                {
                    StartAttack(false);
                }
                else if (isInAI)
                {
                    StartWalk(EndCause.no);
                }
                break;
        }

        Control.UpdateFromUnit();
    }

    private void ReturnHome()
    {
        aiStatus = AIStatus.returnHome;
        if (Action != null)
        {
            Action.End();
        }
        Action = new MoveAction(this, bornPosition, EndComeHome);
    }

    protected virtual void SecondaryAction()
    {

    }

    private void StartWalk(EndCause byHit)
    {
        aiStatus = AIStatus.walk;
        int coef = 60;
        if (Action is MoveAction)
        {
            coef = 30;
        }
        var shalgo = UnityEngine.Random.Range(0, 100) < coef;
        if (shalgo)
        {
            var randPos = new Vector3(bornPosition.x + UnityEngine.Random.Range(-isHomeDist, isHomeDist),
                bornPosition.y,
                bornPosition.z + UnityEngine.Random.Range(-isHomeDist, isHomeDist));
            Action = new MoveAction(this, randPos, StartWalk);
        }
        else
        {
            Action = new StayAction(this, StartWalk);
        }
        
    }

    private void EndComeHome(EndCause byHit)
    {
        isHome = true;
//        if (aiStatus == AIStatus.returnHome)
//            return;
        StartWalk(byHit);
        
    }

    public void Disable()
    {
        isDisabled = true;
        if (Action != null)
        {
            Action.Stop();
            Control.Stop(false);
            Action = null;
            //Maybe here set animation
        }
        aiStatus = AIStatus.disable;
    }
    public void Activate()
    {
        isDisabled = false;
    }

    protected virtual void StartAttack(bool byHit)
    {
        var heroTarget = MainController.Instance.level.MainHero;
        if (heroTarget == null)
        {
            return;
        }
        aiStatus = AIStatus.attack;
        switch (Parameters.AttackType)
        {
            case AttackType.distanceFight:
                Action = new AttackDistance(this, heroTarget, EndAttack, byHit);
                break;
            case AttackType.closeCombat:
                Action = new AttackCloseCombat(this, heroTarget, EndAttack, byHit);
                break;
        }
    }

    private void EndAttack(EndCause obj)
    {
        switch (obj)
        {
            case EndCause.no:
                StartAttack(false);
                break;
            case EndCause.runAway:

                break;
        }
    }

    public bool IsInRadius(float rad)
    {
        return mainHeroDist > 0 && mainHeroDist < rad;
    }

    public void SetDistance(float f)
    {
        mainHeroDist = f;
    }
}

