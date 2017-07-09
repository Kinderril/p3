﻿using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Hero : Unit
{
    public const int HOMING_RAD_SQRT = 44;

//    private const float moneyBonusCoef = 0.2f;
//    private const float moneyBonusCoefTime = 0.8f;
    
    public PSAbsorber GetItemEffect;
    private float currenthBonus = 0f;
    private float currenthBonusTimeLeft = 0f;
    private float regenCoef = 1.2f;
    private HeroControl heorControl;
    public bool isRegenHP = false;
    public ArrowTarget ArrowTarget;
    private ShootContainer shootContainer;
    private RotateContainer rotateContainer;
    public IEnumerable<BonusItem> Bonuses;


    public void Init(Level lvl)
    {
        base.Init();
        var playerData = MainController.Instance.PlayerData;
        var allWeared = playerData.GetAllWearedItems();
        var notBonuses = allWeared.Where(x => x.Slot != Slot.bonus);
        Bonuses = allWeared.Where(x => x.Slot == Slot.bonus).Select(x=>x as BonusItem);
        foreach (var allWearedItem in notBonuses)
        {
            allWearedItem.Activate(this, lvl);
        }
        foreach (var bonuseItem in Bonuses)
        {
            bonuseItem.Activate(this, lvl);
        }

        foreach (ParamType v in Enum.GetValues(typeof(ParamType)))
        {
            Parameters.SetAbsolute(v,playerData.CalcParameter(v));
            Debug.Log("Calc parameter: " + v + " : " + Parameters[v]);
        }
        curHp = Parameters[ParamType.Heath];
        Parameters.SetAbsolute(ParamType.Speed, Parameters[ParamType.Speed] / Formuls.SpeedCoef);
        ; ;
//        Parameters.Parameters[ParamType.PPower] *= (damageBonusFromItem + 1f);
//        Parameters.Parameters[ParamType.MPower] *= (damageBonusFromItem + 1f);
//        GetItemEffect.Stop(true);
        heorControl = Control as HeroControl;
        heorControl.Init(OnRotationEnds);
        Utils.GroundTransform(transform);
    }

    private void OnRotationEnds()
    {
//        Debug.Log("OnRotationEnds " + isPlayAttack);
        if (shootContainer != null)
        {
            var can = curWeapon.CanShoot();
            if (can && !isPlayAttack)
            {
                base.TryAttack(shootContainer.dir,shootContainer.additionalPower);
            }
            shootContainer = null;
        }
    }

    protected override void ShootEnd()
    {
        base.ShootEnd();
//        Debug.Log("ShootEnds");

        if (rotateContainer != null)
        {
            TryAttack(rotateContainer.trg);
            rotateContainer = null;
        }
    }


    protected override void InitWEapons()
    {
        var allWearedItems = MainController.Instance.PlayerData.GetAllWearedItems();
        foreach (var inventoryWeapon in InventoryWeapons)
        {
            PlayerItem additionItem =null;
            switch (inventoryWeapon.Parameters.type)
            {
                case WeaponType.magic:
                    additionItem = allWearedItems.FirstOrDefault(x => x.Slot == Slot.magic_weapon) as PlayerItem;
                    break;
                case WeaponType.physics:
                    additionItem = allWearedItems.FirstOrDefault(x => x.Slot == Slot.physical_weapon) as PlayerItem;
                    break;
            }
            //Debug.Log("Weapon inited: " + additionItem + "   ::" +additionItem.specialAbilities);
            inventoryWeapon.Init(this,additionItem);
        }
    }

    void Update()
    {
        UpdateUnit();
    }

    protected override void UpdateUnit()
    {
        RegenHP();
        if (currenthBonusTimeLeft > 0)
        {
            currenthBonusTimeLeft -= Time.deltaTime;
            if (currenthBonusTimeLeft < 0)
            {
                currenthBonus = 0;
            }
        }
//        Control.UpdateFromUnit();
//        if (Action != null)
//            Action.Update();
        DEbugUpdate();
    }

    private void DEbugUpdate()
    {
        var s = "";
        s += " " +Parameters[ParamType.Heath].ToString("0.0");
        s += " " + Parameters[ParamType.PPower].ToString("0.0");
        s += " " + Parameters[ParamType.MPower].ToString("0.0");
        s += " " + Parameters[ParamType.PDef].ToString("0.0");
        s += " " + Parameters[ParamType.MDef].ToString("0.0");
        s += " " + Parameters[ParamType.Speed].ToString("0.0");
        DebugController.Instance.InfoField1.text = s;
    }

    public void TryAttackByDirection(Vector3 dir, float additionalPower = 0,bool homing = false)
    {
        var p = transform.position;
        var trg = p + dir;


#if UNITY_EDITOR
//        Ray ray = new Ray(p, trg);
        Debug.DrawRay(transform.position, dir * 5, Color.red, 1);
#endif
        if (homing)
        {
            var centr = p + curWeapon.Parameters.range* dir.normalized;
#if UNITY_EDITOR
//            Ray ray2 = new Ray(centr, Vector3.up);
            Debug.DrawRay(centr, Vector3.up * 4, Color.yellow, 2);
#endif
            var closestEnemy = Map.Instance.FindClosesEnemy(centr);
            if (closestEnemy != null)
            {
                var ep = closestEnemy.transform.position;
                var sDist = (ep-centr).sqrMagnitude;
                var shallHoming = sDist < HOMING_RAD_SQRT;
//                Debug.Log("shallHoming:" + shallHoming + "  sDist:" + sDist + "  enemy:" + closestEnemy.name + "  centr:" + centr);
                if (shallHoming)
                {
                    trg = new Vector3(ep.x, p.y, ep.z);
                }
            }
        }
        TryAttack(trg, additionalPower);
    }

    public override void TryAttack(Vector3 target,float additionalPower = 0,Unit unit = null)
    {
        var can = curWeapon.CanShoot();

//        target = transform.position + new Vector3(0, 0, 10);//TODO STUB
//        if (UnityEngine.Random.Range(0, 100) < 50)
//        {
//
//            target = transform.position + new Vector3(10, 0, 10);//TODO STUB
//        }


        var dir = target - transform.position;

        var isLookToTarget = heorControl.SpinTransform.IslookingSame(dir);
//        Debug.Log("Direction from UI:" + dir + "   isLookToTarget:"+ isLookToTarget);
        //        Debug.Log("hero try attack isLookToTarget: " + isLookToTarget);
        //        heorControl.SetLookDir(dir);
        if (can && isLookToTarget)
        {
            base.TryAttack(dir);
        }
        else
        {
            subTR(target, dir, additionalPower);
        }
    }

    private void subTR(Vector3 target,Vector3 dir,float additionalPower)
    {
        if (isPlayAttack)
        {
            rotateContainer = new RotateContainer(target);
        }
        else
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, dir * 5, Color.green, 2);
#endif
            heorControl.SetLookDir(dir);
            shootContainer = new ShootContainer(dir, additionalPower);
        }
    }


    public void GetItems(ItemId type, int count)
    {
        count =(int)(count * (currenthBonus + 1));

//        CurrenthBonus += moneyBonusCoef;
//        currenthBonusTimeLeft += moneyBonusCoefTime;
        
        MainController.Instance.level.AddItem(type, count);
    }

    private void RegenHP()
    {
        if (isRegenHP)
        {
            var p = Time.deltaTime * regenCoef;
            Debug.Log("Regen Delta:" + p);
            SetHp(CurHp + p);
            if (OnGetHit != null)
            {
                OnGetHit(CurHp, Parameters[ParamType.Heath], p);
            }
        }
    }

    public void GetItems(Dictionary<ItemId,int> chest)
    {
        foreach (var item in chest)
        {
            GetItems(item.Key, item.Value);
        }
    }
    
    protected override void Death()
    {
        if (!IsDead)
        {
            base.Death();
            MainController.Instance.level.PreEndLevel(EndlevelType.bad);
        }
    }
    


    public override void GetHit(Bullet bullet)
    {
        base.GetHit(bullet);
        
        if (CurHp > 0 && FlashController != null)
        {
            FlashController.Play();
        }
    }

    public void GetItem(CraftItemType type, int count)
    {
        MainController.Instance.level.AddItem(type, count);
    }

    public void Rage()
    {
        Debug.Log("RAGE!!!!");
        regenCoef = -1f;
        isRegenHP = true;
        Parameters.AddCoef(ParamType.PPower, 1.5f);
        Parameters.AddCoef(ParamType.MPower, 1.5f);
        Parameters.AddCoef(ParamType.MDef, 1.3f);
        Parameters.AddCoef(ParamType.PDef, 1.3f);
        Parameters.AddCoef(ParamType.Speed, 1.3f);
        //TODO add rage effect
    }
}

class ShootContainer
{
    public Vector3 dir;
    public float additionalPower;
    public ShootContainer(Vector3 dir,float additionalPower)
    {
        this.dir = dir;
        this.additionalPower = additionalPower;
    }
}

class RotateContainer
{
    public Vector3 trg;
    public RotateContainer(Vector3 trg)
    {
        this.trg = trg;
    }
    
}