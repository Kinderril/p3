using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Weapon : MonoBehaviour, IBulletHolder
{
    private float nexAttackTime;
    public Bullet bullet;
    public Unit owner;
    public BaseEffectAbsorber pSystemOnShot;
    public WeaponParameters Parameters;
    public PlayerItem PlayerItem;
    public Transform bulletComeOut;
    public const float CHARGE_COEF = 1.4f;
    public const float MAX_CHARGE_TIME = 1.2f;
    public const float CHARGE_TIME_DELAY = 0.5f;

    public void Init(Unit owner,PlayerItem PlayerItem)
    {
        nexAttackTime = 0;
        this.PlayerItem = PlayerItem;
        if (pSystemOnShot != null)
        {
            pSystemOnShot.Stop();
        }
        this.owner = owner;
    }

    public bool CanShoot()
    {
        return Time.time > nexAttackTime;
    }

    public void SetNextTimeShoot()
    {
        nexAttackTime = Time.time + Parameters.attackCooldown;
    }

    public virtual float GetPower()
    {  
        float val = 0;
        switch (Parameters.type)
        {
            case WeaponType.magic:
                val = owner.Parameters.Parameters[ParamType.MPower];
                break;
            case WeaponType.physics:
                val = owner.Parameters.Parameters[ParamType.PPower];
                break;
        }
        return val;
    }

    protected Vector3 GetStartPos()
    {

        Vector3 outPosVector3;
        if (bulletComeOut != null)
        {
            outPosVector3 = bulletComeOut.position;
        }
        else
        {
            outPosVector3 = owner.transform.position;
        }
        return outPosVector3;
    }

    public virtual void DoShoot(Vector3 dir, float additionalPower = 0, Unit target = null)
    {
        additionalPower *= CHARGE_COEF;
        Vector3 outPosVector3 = GetStartPos();
        if (Parameters.isHoming)
        {
            Unit potentialTarget = null;
            if (owner is Hero)
            {
                potentialTarget = Map.Instance.FindClosesEnemy(owner.transform.position);
            }
            else
            {
                potentialTarget = MainController.Instance.level.MainHero;
            }

            var dist = (outPosVector3  - potentialTarget.transform.position).sqrMagnitude;
            if (potentialTarget != null && dist < 30)
            {
                Bullet bullet1 = Instantiate(bullet.gameObject).GetComponent<Bullet>();
                bullet1.transform.position = outPosVector3;
                bullet1.Init(potentialTarget, this);
            }
            else
            {
                Debug.Log("Homing dist is LONG " + dist + " > 30");
            }
        }
        else
        {
            Bullet bullet1 = Instantiate(bullet.gameObject).GetComponent<Bullet>();
            bullet1.transform.position = outPosVector3;
            if (target == null || (target != null && target.IsDead))
            {
                bullet1.Init(dir, this);
            }
            else
            {
                dir = target.transform.position - outPosVector3;
                dir.y = 0;
                bullet1.Init(dir, this);
            }
            bullet1.AdditionalPower = additionalPower;
        }
        if (pSystemOnShot != null)
        {
            pSystemOnShot.Play();
        }
    }

    public SpecialAbility SpecAbility
    {
        get { return PlayerItem == null ? SpecialAbility.none : PlayerItem.specialAbilities; }
    }
    public float Power {
        get
        {
            return GetPower(); 
            
        } 
    }

    public Unit Owner
    {
        get { return owner; }
    }
    public WeaponType DamageType
    {
        get { return Parameters.type; }
    }
}

