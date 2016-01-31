using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Channels;
using UnityStandardAssets.Characters.ThirdPerson;

public enum UnitType
{
    hero,
    monster
}
public class Unit : MonoBehaviour
{
    protected float curHp;
    public Weapon curWeapon;
    public List<Weapon> InventoryWeapons;
    public BaseControl Control;
    private BaseAction action;
    public UnitType unitType;
    public Transform weaponsContainer;
    public event Action<Unit> OnDead;
    public Action<float, float,float> OnGetHit;
    protected bool isDead = false;
    public UnitParameters Parameters;
    private AnimationController animationController;
    public Action<Unit> OnShootEnd;
    public Action<Weapon> OnWeaponChanged;
    protected float lastWeaponChangse;
    public ParticleSystem HitParticleSystem;
    protected bool isPlayAttack = false;
    public float _shield;
    public Action OnShieldOn;
    public IEndEffect OnShieldOff;

    public float CurHp
    {
        get { return curHp; }
        set
        {
            curHp = Mathf.Clamp(value,-1,Parameters.Parameters[ParamType.Hp]);
            if (curHp <= 0)
            {
                Dead();
            }
        }
    }

    protected BaseAction Action
    {
        get { return action; }
        set
        {
            if (action != null)
            {
                action.Dispose();
            }
            action = value;
        }
    }
    public bool IsDead
    {
        get { return isDead; }
    }

    public float Shield
    {
        set
        {
            _shield = value;
            if (_shield > 0)
            {
                var effectVisual = DataBaseController.Instance.Pool.GetItemFromPool(EffectType.shield);
                effectVisual.Init(this, OnShieldOff);

                if (OnShieldOn != null)
                    OnShieldOn();
            }
        }
    }

    public virtual void Init()
    {
        Parameters = Parameters.Copy();
        if (Control == null)
            Control = GetComponent<BaseControl>();
        animationController = GetComponentInChildren<AnimationController>();
        if(animationController == null)
            Debug.LogError("NO ANImator Controller");
        curHp = Parameters.Parameters[ParamType.Hp];
        //List<Weapon> weapons = new List<Weapon>();
        foreach (var inventoryWeapon in InventoryWeapons)
        {
            inventoryWeapon.Init(this, null);
            inventoryWeapon.gameObject.SetActive(false);
        }
        Control.SetSpped(Parameters.Parameters[ParamType.Speed]);
        if (InventoryWeapons.Count == 0)
        {
            Debug.LogWarning("NO WEAPON!!! " + gameObject.name);
            return;
        }
        InitWEapons();
        curWeapon = InventoryWeapons[0];
        curWeapon.gameObject.SetActive(true);
        OnShieldOff = new IEndEffect();
    }

    protected virtual void InitWEapons()
    {
        foreach (var inventoryWeapon in InventoryWeapons)
        {
            inventoryWeapon.Init(this, null);
        }
    }
    
    public virtual void TryAttack(Vector3 direction, Unit target = null)
    {
        if (!isPlayAttack)
        {
//            Debug.Log("hero try attack isLookToTarget: isPlayAttack = true");
            isPlayAttack = true;
//            isPlayAttack = true;
            Control.PlayAttack();
            curWeapon.SetNextTimeShoot();
            animationController.StartPlayAttack(() =>
            {
                curWeapon.DoShoot(direction, target);
                ShootEnd();
            });
        }
    }

    protected virtual void ShootEnd()
    {
//        Debug.Log("End attack  isPlayAttack = false");
        isPlayAttack = false;
        if (OnShootEnd != null)
        {
            OnShootEnd(this);
        }
    }

    public void SwitchWeapon()
    {
        if (Time.time - lastWeaponChangse > 1)
        {
            if (InventoryWeapons.Count <= 1)
                return;
            curWeapon.gameObject.SetActive(false);
            var index = InventoryWeapons.IndexOf(curWeapon);
            index++;
            if (index >= InventoryWeapons.Count)
            {
                index = 0;
            }
            curWeapon = InventoryWeapons[index];
            curWeapon.gameObject.SetActive(true);
            if (OnWeaponChanged != null)
            {
                OnWeaponChanged(curWeapon);
            }
        }
    }

    void Update()
    {
        if (!isDead)
        {
            UpdateUnit();
        }
    }

    protected virtual void UpdateUnit()
    {
        if (action != null)
            action.Update();
    }
    public void MoveToDirection(Vector3 dir)
    {
        Control.MoveTo(dir * Parameters.Parameters[ParamType.Speed]);
    }

    private float calcResist(float curResist)
    {
        return 1 - curResist/(100 + curResist);
    }

    public void GetHit(float power,WeaponType type,float mdef,float pdef)
    {
        switch (type)
        {
            case WeaponType.magic:
                power *= calcResist(mdef);
                break;
            case WeaponType.physics:
                power *= calcResist(pdef);
                break;
        }
        power = GreatRandom.RandomizeValue(power);
        if (_shield > 0)
        {
            if (_shield > power)
            {
                _shield -= power;
                return;
            }
            else
            {
                power -= _shield;
                _shield = 0;
                OnShieldOff.Do();
            }
        }
        CurHp = CurHp - power;
        if (this is Hero)
        {
            if (OnGetHit != null)
            {
                OnGetHit(CurHp, Parameters.Parameters[ParamType.Hp], power);
            }
        }
    }

    public virtual void GetHit(Bullet bullet)
    {
        float power = bullet.weapon.GetPower();
        float mdef = Parameters.Parameters[ParamType.MDef];
        float pdef = Parameters.Parameters[ParamType.PDef];

        if (bullet.weapon.PlayerItem != null)
        {
            //Debug.Log("Test bullet.weapon.PlayerItem.specialAbilities : " + bullet.weapon.PlayerItem.specialAbilities);
            switch (bullet.weapon.PlayerItem.specialAbilities)
            {
                case SpecialAbility.Critical:

                    var isCrit = UnityEngine.Random.Range(0, 10) < 2;
                    //Debug.Log("Test crit : " + isCrit);
                    if (isCrit)
                    {
                        power *= 2.25f;
                    }
                    break;
                case SpecialAbility.push:
                    var owner2 = bullet.weapon.owner;
                    var dir = (transform.position - owner2.transform.position).normalized;

                    break;
                case SpecialAbility.slow:
                    Parameters.Parameters[ParamType.Speed] *= 0.92f;
                    break;
                case SpecialAbility.removeDefence:
                    Parameters.Parameters[ParamType.PDef] *= 0.94f;
                    Parameters.Parameters[ParamType.MDef] *= 0.94f;
                    break;
                case SpecialAbility.vampire:
                    var owner = bullet.weapon.owner;
                    //Debug.Log("BEfore " + owner.CurHp);
                    var diff = power*0.1f;
                    owner.CurHp += diff;
                    if (owner is Hero)
                    {
                        if (owner.OnGetHit != null)
                        {
                            owner.OnGetHit(owner.CurHp, owner.Parameters.Parameters[ParamType.Hp], -diff);
                        }
                    }
                    break;
                case SpecialAbility.clear:
                    mdef = mdef/2;
                    pdef = pdef/2;
                    break;
            }
            if (HitParticleSystem != null)
            {
                HitParticleSystem.Play();
            }
        }
        GetHit(power, bullet.weapon.Parameters.type, mdef, pdef);
    }
    

    

    protected virtual void Dead()
    {
        if (OnDead != null)
        {
            OnDead(this);
        }
        var collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;
        var rigbody = GetComponent<Rigidbody>();
        if (rigbody != null)
            rigbody.isKinematic = true;
        isDead = true;
        Control.SetDeath();
        
        StartCoroutine(PLayDeath());
    }

    private IEnumerator PLayDeath()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void DeInit()
    {
        isDead = true;
    }

}
