﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Hero : Unit
{
    private const float moneyBonusCoef = 0.2f;
    private const float moneyBonusCoefTime = 0.8f;
    public PSAbsorber GetItemEffect;
    private float currenthBonus = 0f;
    private float currenthBonusTimeLeft = 0f;
    public Action<float> CurrentBonusUpdateX;
    public float moneyBonusFromItem = 0.0f;
    public float damageBonusFromItem = 0.0f;
    private HeroControl heorControl;
    public bool isRegenHP = false;
    public ArrowTarget ArrowTarget;
    private ShootContainer shootContainer;
    private RotateContainer rotateContainer;

    public float CurrenthBonus
    {
        get { return currenthBonus; }
        set
        {
            currenthBonus = value;
            if (CurrentBonusUpdateX != null)
            {
                CurrentBonusUpdateX( currenthBonus );
            }
        }
    }

    public override void Init()
    {
        base.Init();
        var playerData = MainController.Instance.PlayerData;
        foreach (ParamType v in Enum.GetValues(typeof(ParamType)))
        {
            Parameters.Parameters[v] = playerData.CalcParameter(v);
            Debug.Log("Calc parameter: " + v + " : " + Parameters.Parameters[v]);
        }
        curHp = Parameters.Parameters[ParamType.Heath];

        Parameters.Parameters[ParamType.PPower] *= damageBonusFromItem + 1f;
        Parameters.Parameters[ParamType.MPower] *= damageBonusFromItem + 1f;
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
        Control.UpdateFromUnit();
        if (Action != null)
            Action.Update();
    }
    public void TryAttackByDirection(Vector3 dir, float additionalPower = 0)
    {
        var trg = transform.position + dir;


#if UNITY_EDITOR
        Ray ray = new Ray(transform.position, trg);
        Debug.DrawRay(ray.origin, ray.direction * 5, Color.red, 1);
#endif

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

        var isLookToTarget = heorControl.SpinTransform.ShallRotate(dir);
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
            Debug.DrawRay(transform.position, dir * 5, Color.yellow, 2);
#endif
            heorControl.SetLookDir(dir);
            shootContainer = new ShootContainer(dir, additionalPower);
        }
    }


    public void GetItems(ItemId type, int count)
    {
        count =(int)(count * (currenthBonus + 1));

        CurrenthBonus += moneyBonusCoef;
        currenthBonusTimeLeft += moneyBonusCoefTime;
        
        MainController.Instance.level.AddItem(type, count);
    }

    private void RegenHP()
    {
        if (isRegenHP)
        {
            var p = Time.deltaTime * 0.2f;
            CurHp = CurHp + p;
            if (OnGetHit != null)
            {
                OnGetHit(CurHp, Parameters.Parameters[ParamType.Heath], p);
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
            MainController.Instance.EndLevel(EndlevelType.bad);
        }
    }

    public void GetHeal(float currentPower)
    {
        var effect = DataBaseController.Instance.Pool.GetItemFromPool(EffectType.heal);
        effect.Init(this,3.5f);
        var p =  currentPower;
        CurHp += p;
        if (OnGetHit != null)
        {
            OnGetHit(CurHp, Parameters.Parameters[ParamType.Heath], p);
        }
    }

    public override void GetHit(Bullet bullet)
    {
        base.GetHit(bullet);
    }

    public void GetItem(CraftItemType type, int count)
    {
        MainController.Instance.level.AddItem(type, count);
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