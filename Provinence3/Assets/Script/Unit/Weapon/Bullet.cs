using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 0.002f;
    private float time = 0;
    public Vector3 trg;
    public Vector3 start;
    public Unit targetUnit;
    public Weapon weapon;
    private Action updateAction;
    public BaseEffectAbsorber TrailParticleSystem;
    public BaseEffectAbsorber HitParticleSystem;
    protected List<Unit> AffecttedUnits = new List<Unit>();
    public bool rebuildY = true;
//    private UnitType ownerType;

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
            direction = new Vector3(direction.x, transform.position.y, direction.z);
        }
        speed = weapon.Parameters.bulletSpeed;
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
    
    public void Init(Unit target, Weapon weapon)
    {
        speed = weapon.Parameters.bulletSpeed;
        targetUnit = target;
        start = transform.position;
        this.weapon = weapon;
        subInit();
        updateAction = updateTargetUnit;
        transform.LookAt(targetUnit.transform.position);
    }

    private void subInit()
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
        if (weapon != null && weapon.PlayerItem != null)
        {
            switch (weapon.PlayerItem.specialAbilities)
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
        Debug.Log("update vector " + time + "   " + transform.position + "   " + targetUnit  +  "   s:" + start);
        transform.position = Vector3.Lerp(start, targetUnit.transform.position, time);
        if (time > 1)
        {
            Death();
        }
    }

    void FixedUpdate()
    {
        if (updateAction != null)
        {
            updateAction();
        }
    }
}

