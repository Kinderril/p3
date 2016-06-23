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
    public float attackDist = 45;
    public const float AI_DIST = 170;
    private const float runAwayDist = 110;
    public float mainHeroDist = 0;
    public Vector3 bornPosition;
    public AIStatus aiStatus;
    private Hero mainHero;
    public int moneyCollect;
    public int energyadd = 4;
    private bool isHome = true;
    private BaseAction attackBehaviour;
    private bool isDisabled = false;
    public bool haveAction;
    public List<DropItem> dropItems; 
    public FlashController FlashController;
    

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
        //runAwayDist = attackDist * 1.4f;
        base.Init();
        Parameters.Parameters[ParamType.Speed] = GreatRandom.RandomizeValue(Parameters.Parameters[ParamType.Speed]);
        bornPosition = transform.position;
        Utils.GroundTransform(transform, 999f);
        //curWeapon.power = GreatRandom.RandomizeValue(curWeapon.power);
        moneyCollect = GreatRandom.RandomizeValue(moneyCollect);
        energyadd = GreatRandom.RandomizeValue(energyadd);
        aiStatus = AIStatus.disable;
    }

    protected override void Death()
    {
        Control.Dead();
        MainController.Instance.level.AddItem(ItemId.energy, -energyadd);
        
        DropItems();
        DropMoney();

        base.Death();
        Action = null;
    }

    private void DropItems()
    {

        foreach (var dropItem in dropItems)
        {
            var isDrop = UnityEngine.Random.Range(0f, 1f) < dropItem.chance0_1;
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
        var itemMapItem = DataBaseController.GetItem<ItemMapItem>(DataBaseController.Instance.ItemMapItemPrefab, transform.position);
        itemMapItem.Init(t);
        itemMapItem.transform.SetParent(Map.Instance.miscContainer, true);
        itemMapItem.StartFly(transform);
    }


    private void DropMoney()
    {
        var goldMapItem = DataBaseController.GetItem<GoldMapItem>(DataBaseController.Instance.GoldMapItemPrefab, transform.position);
        goldMapItem.Init(ItemId.money, moneyCollect);
        goldMapItem.transform.SetParent(Map.Instance.miscContainer, true);
        goldMapItem.StartFly(transform);
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
    
    public void CheckDistance(float heroDist)
    {
        if (mainHero == null)
            return;

        mainHeroDist = (mainHero.transform.position - transform.position).sqrMagnitude;
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

}

