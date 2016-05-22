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
    private float runAwayDist = 110;
    public const float AI_DIST = 170;
    public float mainHeroDist = 0;
    public Vector3 bornPosition;
    private AIStatus aiStatus;
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
            var fn = DataBaseController.Instance.Pool.GetItemFromPool<FlyNumberWIthDependence>(PoolType.flyNumberInGame);
            fn.transform.SetParent(WindowManager.Instance.CurrentWindow.TopPanel.transform);
            //fn.transform.position = transform.position;
            fn.Init(transform, "-" + hp.ToString("0"),Color.red);
            if (FlashController != null)
                FlashController.Play();
            GlobalEventManager.Instance.MonsterGetHit(this);
        }
        if (attackBehaviour == null)
        {
            StartAttack(true);
        }

    }

//    protected override void UpdateUnit()
//    {
////        if (!isDead && !isDisabled)
////        {
////            CheckDistance();
//            base.UpdateUnit();
////        }
//    }


    public void CheckDistance(float heroDist)
    {
        if (mainHero == null)
            return;

        mainHeroDist = (mainHero.transform.position - bornPosition).sqrMagnitude;
        
        if (mainHeroDist < AI_DIST)
        {
            isHome = false;
            Control.UpdateFromUnit();
            bool isTargetClose = (mainHeroDist < attackDist);
            switch (aiStatus)
            {
                case AIStatus.disable:
                    StartWalk(false);
                    break;
                case AIStatus.attack:
                    if ((mainHeroDist > runAwayDist))
                    {
                        EndAttack();
                    }
                    break;
                case AIStatus.returnHome:
                    if (isTargetClose)
                    {
                        StartAttack(false);
                    }
                    break;
                case AIStatus.walk:
                    if (isTargetClose)
                    {
                        StartAttack(false);
                    }
                    break;
                case AIStatus.secondaryAction:
                    SecondaryAction();
                    break;
            }
        }
        else
        {
            if (aiStatus != AIStatus.returnHome && !isHome)
            {
//                Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>Start go home 1111111111111 prevStatus " + aiStatus + "    mainHeroDist " + mainHeroDist + "   aiDist:" + aiDist);
//                aiStatus = AIStatus.disable;
                //                Action = null;
                EndAttack();
            }
        }
    }

    protected virtual void SecondaryAction()
    {

    }

    private void StartWalk(bool byHit)
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

    private void EndWalk(bool byHit)
    {
        isHome = true;
        if (aiStatus == AIStatus.returnHome)
            return;
        StartWalk(false);
        
    }

    public void Disable()
    {
        isDisabled = true;
        if (Action != null)
        {
//            Action.Stop();
            Control.Stop(false);
            Action = null;
            //Maybe here set animation
        }
    }
    public void Activate()
    {
        isDisabled = false;
    }

    protected virtual void StartAttack(bool byHit)
    {
        aiStatus = AIStatus.attack;
        switch (Parameters.AttackType)
        {
            case AttackType.distanceFight:
                Action = new AttackDistance(this, MainController.Instance.level.MainHero, StartAttack, byHit);
                break;
            case AttackType.closeCombat:
                Action = new AttackCloseCombat(this, MainController.Instance.level.MainHero, StartAttack, byHit);
                break;
        }
    }
    public bool IsInRadius(float rad)
    {
        return mainHeroDist < rad;
    }

    private void EndAttack()
    {
//        Debug.Log("Run AWAY  " + Parameters.AttackType);
        aiStatus = AIStatus.returnHome;
        if (Action != null)
        {
            Action.End();
        }
        Action = new MoveAction(this, bornPosition, EndWalk);
    }
}

