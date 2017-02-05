using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UnitType
{
    hero,
    monster
}
public class Unit : MonoBehaviour
{
    public Action<Unit> OnShootEnd;
    public event Action OnUnitDestroy;
    public event Action<Vector3> OnUnitAttack;
    public Action<Weapon> OnWeaponChanged;
    public Action OnShieldOn;
    public event Action<Unit> OnDead;
    public Action<float, float, float> OnGetHit;

    protected float curHp;
    public Weapon curWeapon;
    public List<Weapon> InventoryWeapons;
    public BaseControl Control;
    private BaseAction action;
    public UnitType unitType;
    public Transform weaponsContainer;
    protected bool isDead = false;
    public UnitParameters ParametersScriptable;
    public UnitParametersInGame Parameters;
    private AnimationController animationController;
    protected float lastWeaponChangse;
    public ParticleSystem HitParticleSystem;
    public BaseEffectAbsorber StartAttackEffect;
    protected bool isPlayAttack = false;
    public float _shield;
    public IEndEffect OnShieldOff;
    public List<TimeEffect> efftcs = new List<TimeEffect>();
    public DeathInfo LastHitInfo;
    public FlashController FlashController;

    public float CurHp
    {
        get { return curHp; }
//        set
//        {
//            curHp = Mathf.Clamp(value,-1,Parameters[ParamType.Heath]);
//            if (curHp <= 0)
//            {
//                Death();
//            }
//        }
    }

    public void SetHp(float val)
    {
        curHp = Mathf.Clamp(val, -1, Parameters[ParamType.Heath]);
        if (curHp <= 0)
        {
            Death();
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
            var prev = _shield <= 0;
            _shield = value;
            if (_shield > 0)
            {
                if (prev)
                {
                    var effectVisual =
                        DataBaseController.Instance.Pool.GetItemFromPool(EffectType.shield) as VisualEffectBehaviour;
                    effectVisual.Init(this, OnShieldOff);
                }
                if (OnShieldOn != null)
                    OnShieldOn();
            }
        }
        get { return _shield; }
    }

    public virtual void Init()
    {
        Parameters = ParametersScriptable.Get();
        var spd = Parameters[ParamType.Speed];
        if (spd != 0 && spd < 50)
        { 
            spd *= Formuls.SpeedCoef;
            Debug.LogError("Wrong speed " + name);
        }
        Parameters[ParamType.Speed] = spd / Formuls.SpeedCoef;
        if (Control == null)
            Control = GetComponent<BaseControl>();
        if (animationController == null)
            animationController = GetComponentInChildren<AnimationController>();
        if(animationController == null)
            Debug.LogError("NO ANImator Controller");
        curHp = Parameters[ParamType.Heath];
        //List<Weapon> weapons = new List<Weapon>();
        foreach (var inventoryWeapon in InventoryWeapons)
        {
            inventoryWeapon.Init(this, null);
            inventoryWeapon.gameObject.SetActive(false);
            inventoryWeapon.transform.SetParent(weaponsContainer,true);
        }
        Control.SetSpeed(Parameters[ParamType.Speed]);
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
    
    public virtual void TryAttack(Vector3 direction, float additionalPower = 0, Unit target = null)
    {
        if (!isPlayAttack && direction.sqrMagnitude > 0)
        {
            isPlayAttack = true;
//            Debug.Log("Start Attack : " + direction);
            Control.PlayAttack();
            if (StartAttackEffect != null)
            {
//                Debug.Log("NSParticleAbsorber PLAY " + gameObject.name);
                StartAttackEffect.Play();
            }
            if (OnUnitAttack != null)
            {
                OnUnitAttack(direction);
            }
            curWeapon.SetNextTimeShoot();
            animationController.StartPlayAttack(() =>
            {
                curWeapon.DoShoot(direction, additionalPower, target);
                ShootEnd();
                if (StartAttackEffect != null)
                {
                    StartAttackEffect.Stop();
                }
            });
        }
    }

    protected virtual void ShootEnd()
    {
//        Debug.Log("End attack ");
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
            if (curWeapon.pSystemOnShot != null)
                curWeapon.pSystemOnShot.Stop();
            if (OnWeaponChanged != null)
            {
                OnWeaponChanged(curWeapon);
            }
        }
    }

    public void UpdateByManager()
    {
        UpdateUnit();
    }

    protected virtual void UpdateUnit()
    {
        if (action != null)
            action.Update();
    }
    public void MoveToDirection(Vector3 dir)
    {
        Control.MoveTo(dir * Parameters[ParamType.Speed]);
    }
    
    public void GetHit(float power,WeaponType type,DeathInfo info,float mdef = -1,float pdef = -1)
    {
        LastHitInfo = info;
        if (mdef < 0 || pdef < 0)
        {
            mdef = Parameters[ParamType.MDef];
            pdef = Parameters[ParamType.PDef];
        }
        switch (type)
        {
            case WeaponType.magic:
                power *= Formuls.calcResist(mdef);
                break;
            case WeaponType.physics:
                power *= Formuls.calcResist(pdef);
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
        SetHp(CurHp - power);
        if (this is Hero)
        {
            if (OnGetHit != null)
            {
                OnGetHit(CurHp, Parameters[ParamType.Heath], -power);
            }
        }
    }

    public virtual void GetHit(Bullet bullet)
    {
        if (Parameters == null)
        {
            return;
        }
        var addPower = 1 + Mathf.Clamp(bullet.AdditionalPower, 0, Weapon.MAX_CHARGE_TIME);
        float power = bullet.bulletHolder.Power * addPower;
        float mdef = Parameters[ParamType.MDef];
        float pdef = Parameters[ParamType.PDef];

        if (bullet.bulletHolder != null)
        {
            var owner = bullet.bulletHolder.Owner;
            //Debug.Log("Test bullet.weapon.PlayerItem.specialAbilities : " + bullet.weapon.PlayerItem.specialAbilities);
            switch (bullet.bulletHolder.SpecAbility)
            {
                case SpecialAbility.critical:
                    var isCrit = UnityEngine.Random.Range(0, 100) < Formuls.CRIT_CHANCE;
                    if (isCrit)
                    {
                        power *= Formuls.CRIT_COEF;
                    }
                    break;
                case SpecialAbility.push:
//                    var owner2 = bullet.weapon.Owner;
//                    var dir = (transform.position - owner2.transform.position).normalized;
                    //TODO
                    break;
                case SpecialAbility.slow:
                    Parameters[ParamType.Speed] *= Formuls.SLOW_COEF;
                    Control.SetSpeed(Parameters[ParamType.Speed]);
                    break;
                case SpecialAbility.removeDefence:
                    Parameters[ParamType.PDef] *= Formuls.REMOVE_DEF_COEF;
                    Parameters[ParamType.MDef] *= Formuls.REMOVE_DEF_COEF;
                    break;
                case SpecialAbility.vampire:
                    var diff = power*Formuls.VAMP_COEF;
                    SetHp(CurHp + diff);
                    if (owner is Hero)
                    {
                        var isMax = owner.CurHp < owner.Parameters.MaxHp;
                        if (owner.OnGetHit != null && !isMax)
                        {
                            owner.OnGetHit(owner.CurHp, owner.Parameters[ParamType.Heath], diff);
                        }
                    }
                    break;
                case SpecialAbility.clear:
                    mdef = mdef * Formuls.CLEAR_COEF;
                    pdef = pdef * Formuls.CLEAR_COEF;
                    break;
                case SpecialAbility.dot:
                    break;
                case SpecialAbility.distance:
                    var sqrdist = 1 + (owner.transform.position - transform.position).magnitude *  (Formuls.DISTANCE_COEF);
                    power *= Mathf.Clamp(sqrdist,Formuls.DISTANCE_MIN,Formuls.DISTANCE_MAX);
                    break;
                case SpecialAbility.hp:
                    var c = ( 1 - CurHp/Parameters.MaxHp)/Formuls.HP_SKILL_COEF;
                    power *= c;
                    break;
                case SpecialAbility.shield:
                    owner.Shield += MainController.Instance.level.difficult;
                    break;
                case SpecialAbility.stun:
                    var isStun = UnityEngine.Random.Range(0, 100) < Formuls.CHANCE_STUN;
#if UNITY_EDITOR
                    if (DebugController.Instance.CHANCE_STUN_100)
                    {
                        isStun = true;
                    }
#endif
                    Debug.Log("STUN! " + isStun);
                    if (isStun)
                    {
                        TimeEffect.Creat(this, EffectType.freez,0, Formuls.STUN_TIME_SEC);
                    }
                    break;
            }
            if (HitParticleSystem != null)
            {
                HitParticleSystem.Play();
            }
        }
        SourceType sourceType = SourceType.talisman;
        if (bullet.bulletHolder is Weapon)
        {
            sourceType = SourceType.weapon;
        }

        GetHit(power, bullet.bulletHolder.DamageType, new DeathInfo(power, bullet.bulletHolder.DamageType, sourceType), mdef, pdef);
    }
    
    protected virtual void Death()
    {
        if (IsDead)
        {
            Debug.LogError(gameObject.name + " is already dead");
            return;
        }
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
//        base.Death();
//        StartCoroutine(PLayDeath());
    }

//    private IEnumerator PLayDeath()
//    {
//        yield return new WaitForSeconds(5);
//        Destroy(gameObject);
//    }

    public void DeInit()
    {
        isDead = true;
    }

    void OnDestroy()
    {
        if (OnUnitDestroy != null)
        {
            OnUnitDestroy();
        }
    }

}
