﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0.002f;
    protected float time = 0;
    protected Vector3 trg;
    protected Vector3 start;
    private Unit targetUnit;
    public IBulletHolder weapon;
    protected Action updateAction;
    public BaseEffectAbsorber TrailParticleSystem;
    public BaseEffectAbsorber HitParticleSystem;
    protected List<Unit> AffecttedUnits = new List<Unit>();
    public bool rebuildY = true;
    private float additionalPower = 0;
    private float startDist2target;

    public float AdditionalPower
    {
        get { return additionalPower; }
        set { additionalPower = value; }
    }

    void Awake()
    {
        if (HitParticleSystem != null)
        {
            HitParticleSystem.Stop();
        }
    }
    public virtual void Init(Vector3 direction,Weapon weapon)
    {
        if (rebuildY)
        {
            direction = new Vector3(direction.x, 0, direction.z);
        }
//        ownerType = weapon.owner.unitType;
        this.weapon = weapon;
        if (weapon.bulletComeOut != null)
        {
            start = weapon.bulletComeOut.position;
        }
        else
        {
            start = transform.position;
            Debug.Log("wrong bullet start position");
        }
        trg = direction.normalized * weapon.Parameters.range + start;
        subInit();
        updateAction = updateVector;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    public virtual void Init(Unit target, IBulletHolder weapon,Vector3 startPosition)
    {
        targetUnit = target;
        start = startPosition;
        transform.position = start;
        this.weapon = weapon;
        subInit();
        startDist2target = (targetUnit.transform.position - start).magnitude;
        updateAction = updateTargetUnit;
        transform.LookAt(targetUnit.transform.position);
    }

    protected void subInit()
    {
        time = 0;
        if (TrailParticleSystem != null)
        {
            TrailParticleSystem.Play();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        OnBulletHit(other);
    }

    protected virtual void OnBulletHit(Collider other)
    {
        Debug.LogError("DON'T USE THIS CLASSS");
    }

    protected virtual void Hit(Unit unit)
    {
        bool haveManyTargets = false;
        if (weapon != null && weapon.SpecAbility != null)
        {
            switch (weapon.SpecAbility)
            {
                case SpecialAbility.penetrating:
                    haveManyTargets = true;
                    if (AffecttedUnits.Count > 3)
                    {
                        Death();
                    }
                    else
                    {
                        AffecttedUnits.Add(unit);
                        unit.GetHit(this);
                        return;
                    }
                    break;
                case SpecialAbility.chain:
                    //TODO find another target
                    haveManyTargets = true;
                    if (AffecttedUnits.Count > 3)
                    {
                        Death();
                    }
                    else
                    {
                        AffecttedUnits.Add(unit);
                        unit.GetHit(this);
                        return;
                    }
                    break;
            }
        }

        if (!haveManyTargets)
        {
            if (AffecttedUnits.Count < 1)
            {
                AffecttedUnits.Add(unit);
                unit.GetHit(this);
            }
        }
        Death();
    }

    private void Death()
    {
        Destroy(gameObject);
        if (HitParticleSystem != null)
        {
            HitParticleSystem.Play();
            Map.Instance.LeaveEffect(HitParticleSystem);
        }
        if (TrailParticleSystem != null)
        {
            TrailParticleSystem.Stop();
            Map.Instance.LeaveEffect(TrailParticleSystem);
        }
        
    }

    protected void updateVector()
    {
        time += speed;
        transform.position = Vector3.Lerp(start, trg, time);
        if (time > 1)
        {
            Death();
        }
    }

    private void updateTargetUnit()
    {
        time += speed;
        var trgPos = targetUnit.weaponsContainer.position;
        transform.position = Vector3.Lerp(start, trgPos, time);
        var curDist = (start - trgPos).magnitude;
        var curTime = curDist/startDist2target;
        Debug.Log("t: " + time + "     " + curTime);
        if (time > curTime)
        {
            TryHitTragetUnitAndDEath();
        }
    }

    protected void TryHitTragetUnitAndDEath()
    {
        if (targetUnit != null && !targetUnit.IsDead)
        {
            AffecttedUnits.Add(targetUnit);
            targetUnit.GetHit(this);
        }
        Death();
    }

    void FixedUpdate()
    {
        if (updateAction != null)
        {
            updateAction();
        }
    }
}

