using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum AIStatus
{
    attack,
    returnHome,
    walk,
    disable,
}

public class BaseMonster : Unit
{
    private const float isHomeDist = 2;
    public float attackDist = 45;
    private float runAwayDist = 110;
    private const float aiDist = 110;
    public float mainHeroDist = 0;
    public Vector3 bornPosition;
    private AIStatus aiStatus;
    private Hero mainHero;
    public int moneyCollect;
    public int energyadd = 4;
    private bool isHome = true;
    private BaseAction attackBehaviour;
//    public ParticleSystem ParticleSystemBorn;
    public bool haveAction;


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

    protected override void Dead()
    {
        Control.Dead();
        MainController.Instance.level.AddItem(ItemId.energy, -energyadd);
        var mapItem2 = DataBaseController.GetItem<MapItem>(DataBaseController.Instance.MapItemPrefab, transform.position);
        mapItem2.Init(ItemId.money, moneyCollect);
        mapItem2.transform.SetParent(Map.Instance.miscContainer, true);
        mapItem2.StartFly(transform);
        base.Dead();
        Action = null;

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
            //fn.transform.LookAt(MainController.Instance.MainCamera.transform);
        }
        if (attackBehaviour == null)
        {
            StartAttack();
        }

    }

    protected override void UpdateUnit()
    {
        if (!isDead)
        {
            CheckDistance();
            base.UpdateUnit();
        }
    }


    public void CheckDistance()
    {
        if (mainHero == null)
            return;

        mainHeroDist = (mainHero.transform.position - bornPosition).sqrMagnitude;
        
        if (mainHeroDist < aiDist)
        {
            isHome = false;
            Control.UpdateFromUnit();
            bool isTargetClose = (mainHeroDist < attackDist);
            switch (aiStatus)
            {
                case AIStatus.disable:
                    StartWalk();
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
                        StartAttack();
                    }
                    else
                    {
//                        isHome = (transform.position - bornPosition).sqrMagnitude < isHomeDist;
//                        if (isHome)
//                        {
//                            StartWalk();
//                        }
                    }
                    break;
                case AIStatus.walk:
                    if (isTargetClose)
                    {
                        StartAttack();
                    }
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

    private void StartWalk()
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

    private void EndWalk()
    {
        isHome = true;
        if (aiStatus == AIStatus.returnHome)
            return;
        StartWalk();
        
    }

    private void StartAttack()
    {
        aiStatus = AIStatus.attack;
        switch (Parameters.AttackType)
        {
            case AttackType.hitAndRun:
                Action = new AttackHitAndRun(this, MainController.Instance.level.MainHero, StartAttack);
                break;
            case AttackType.distanceFight:
                Action = new AttackDistance(this, MainController.Instance.level.MainHero, StartAttack);
                break;
            case AttackType.closeCombat:
                Action = new AttackCloseCombat(this, MainController.Instance.level.MainHero, StartAttack);
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

