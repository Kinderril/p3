using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum HitPosition
{
    target,
    bullet,
}
public enum ControlPointOffset
{
    none,
    soft,
    hard,
}

public enum FlyType
{
    straight,
    curve
}

public class Bullet : PoolElement
{
    public float speed = 0.002f;
    public int maxTargets = 1;
    protected float time = 0;
    protected Vector3 trg;
    protected Vector3 start;
    protected Vector3 control;
    private Unit targetUnit;
    public IBulletHolder bulletHolder;
    protected Action updateAction;
    public HitPosition hitPOsition;
    public BaseEffectAbsorber TrailParticleSystem;
    public BaseEffectAbsorber HitParticleSystem;
    protected List<Unit> AffecttedUnits = new List<Unit>();
    public bool rebuildY = true;
    public bool playHitAnyway = true;
    private float additionalPower = 0;
    private float startDist2target;
    public FlyType FlyType = FlyType.straight;
    public int ID;
//    protected Weapon weapon;

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
    public virtual void Init(Vector3 direction, IBulletHolder weapon)
    {
        if (rebuildY)
        {
            direction = new Vector3(direction.x, 0, direction.z);
        }
        this.bulletHolder = weapon;
        start = FindStartPos(weapon);
        trg = direction.normalized * weapon.Range + start;
        subInit();
        switch (FlyType)
        {
            case FlyType.straight:
                updateAction = updateVector;
                break;
            case FlyType.curve:
                control = ContrtolPoint(start, trg, ControlPointOffset.soft);
                updateAction = updateQuadVector;
                break;
            default:
                updateAction = updateVector;
                break;
        }
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    protected virtual Vector3 FindStartPos(IBulletHolder weapon)
    {
        Vector3 sPos;
        if (weapon != null)
        {
            if (weapon.BulletComeOut != null)
            {
                sPos = weapon.BulletComeOut.position;
            }
            else
            {
                sPos = weapon.Transform.position;
            }
        }
        else
        {
            sPos = transform.position;
            Debug.Log("wrong bullet start position " + start);
        }
        return sPos;
    }
    
    public virtual void Init(Unit target, IBulletHolder weapon,Vector3 startPosition)
    {
        targetUnit = target;
        start = startPosition;
        transform.position = start;
        this.bulletHolder = weapon;
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
            TrailParticleSystem.transform.SetParent(transform,false);
            TrailParticleSystem.transform.localPosition = Vector3.zero;
            TrailParticleSystem.Play();
        }
        if (HitParticleSystem != null)
        {
            HitParticleSystem.transform.SetParent(transform, false);
            HitParticleSystem.transform.localPosition = Vector3.zero;
            HitParticleSystem.Stop();
        }
        base.Init();
    }

    public override void EndUse()
    {
        targetUnit = null;
        bulletHolder = null;
        updateAction = null;
        time = 0;
        AffecttedUnits.Clear();

        base.EndUse();
    }

    void OnTriggerEnter(Collider other)
    {
        OnBulletHit(other);
    }

    protected virtual void OnBulletHit(Collider other)
    {
        var trg = other.GetComponent<Unit>();
        var spell = bulletHolder as SpellInGame;
        var isHeroOwner = bulletHolder.Owner is Hero;
        if (trg is Hero)
        {
            if (spell != null)
            {
                switch (spell.sourseItem.SpellData.IsPositive())
                {
                    case EffectPositiveType.Positive:
                        if (isHeroOwner)
                        {
                            Hit(trg);
                        }
                        break;
                    case EffectPositiveType.Negative:
                        if (!isHeroOwner)
                        {
                            Hit(trg);
                        }
                        break;
                }

            }
            else if (!isHeroOwner)
            {
                Hit(trg);
            }
        }
        else if (trg is BaseMonster)
        {
            if (spell != null)
            {
                switch (spell.sourseItem.SpellData.IsPositive())
                {
                    case EffectPositiveType.Positive:
                        if (!isHeroOwner)
                        {
                            Hit(trg);
                        }
                        break;
                    case EffectPositiveType.Negative:
                        if (isHeroOwner)
                        {
                            Hit(trg);
                        }
                        break;
               }
            }
            else if (isHeroOwner)
            {
                Hit(trg);
            }
        }
    }

    protected virtual void Hit(Unit unit)
    {
        bool haveManyTargets = false;
        if (!AffecttedUnits.Contains(unit))
        {
            AffecttedUnits.Add(unit);
            unit.GetHit(this);
        }
        if (AffecttedUnits.Count >= maxTargets)
        {
            Death(unit);
        }
    }

    protected void Death(Unit lastUnitHitted)
    {
        var spell = bulletHolder as SpellInGame;
        if (spell != null)
        {
            Debug.Log("sadsa");
            return;
        }
        if (HitParticleSystem != null)
        {
            if (playHitAnyway)
            {
                HitParticleSystem.Play();
                switch (hitPOsition)
                {
                    case HitPosition.target:
                        if (lastUnitHitted != null)
                        {
                            HitParticleSystem.transform.SetParent(lastUnitHitted.transform, true);
                            HitParticleSystem.transform.localPosition = Vector3.zero;
                            if (HitParticleSystem.gameObject != null)
                                MainController.Instance.StartCoroutine(HitParticleSystem.DestroyPS(transform,4,"2"));
                        }
                        break;
                    case HitPosition.bullet:
                        Map.Instance.LeaveEffect(HitParticleSystem, transform);
                        break;
                }
            }
        }
        if (TrailParticleSystem != null)
        {
            TrailParticleSystem.Stop();
            Map.Instance.LeaveEffect(TrailParticleSystem, transform);
        }

        EndUse();
    }

    protected void updateVector()
    {
        time += speed;
        transform.position = Vector3.Lerp(start, trg, time);
        if (time > 1)
        {
            Death(null);
        }
    }
    protected void updateQuadVector()
    {
        time += speed;

        float vn = 1 - time;
        var v2 = vn*vn;
        var t2 = time * time;
        float px = v2 * start.x + 2 * time * vn * control.x + t2 * trg.x;
        float py = v2 * start.y + 2 * time * vn * control.y + t2 * trg.y;
        float pz = v2 * start.z + 2 * time * vn * control.z + t2 * trg.z;
//        float y = start.y*vn + trg.y*time;

        transform.position = new Vector3(px, py, pz); 


        if (time > 1)
        {
            Death(null);
        }
    }

    private Vector3 ContrtolPoint(Vector3 startPos, Vector3 end, ControlPointOffset offset = ControlPointOffset.soft)
    {
        float deltaOffset = 1f;

        var milldePoint = (startPos + end) / 2;
        var alpha = (startPos.y - end.y) / (startPos.x - end.x);
        var dist = (startPos - end).magnitude;
        var v = (new Vector3(1, -1 / alpha)).normalized;


        switch (offset)
        {
            case ControlPointOffset.none:
                deltaOffset = 0.1f;
                break;
            case ControlPointOffset.soft:
                deltaOffset = Mathf.Sign(UnityEngine.Random.value - 0.5f) * (dist * 0.25f + UnityEngine.Random.value * dist * 0.75f);
                break;
            case ControlPointOffset.hard:
                deltaOffset = Mathf.Sign(UnityEngine.Random.value - 0.5f) * (dist * 0.75f + UnityEngine.Random.value * dist * 1.25f);
                break;
        }

        var contrtolPoint = milldePoint + v * deltaOffset;
        Debug.Log(startPos + "   " + contrtolPoint + "   " + end);
        contrtolPoint.z = milldePoint.z;
        return contrtolPoint;
    }

    private void updateTargetUnit()
    {
        time += speed;
        if (targetUnit != null)
        {

            Vector3 trgPos;
            if (targetUnit.weaponsContainer == null)
            {
                trgPos = targetUnit.transform.position + Vector3.up;
            }
            else
            {
                trgPos = targetUnit.weaponsContainer.position;
            }
            transform.position = Vector3.Lerp(start, trgPos, time);
            var curDist = (start - trgPos).magnitude;
            var curTime = curDist/startDist2target;
            if (time > curTime)
            {
                TryHitTragetUnitAndDEath();
            }
        }
        else
        {
            Death(null);
        }
    }

    protected void TryHitTragetUnitAndDEath()
    {
        if (targetUnit != null && !targetUnit.IsDead)
        {
            AffecttedUnits.Add(targetUnit);
            targetUnit.GetHit(this);
        }
        Death(targetUnit);
    }

    void FixedUpdate()
    {
        if (IsUsing && updateAction != null)
        {
            updateAction();
        }
    }
}

