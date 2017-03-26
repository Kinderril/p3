using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BaseBulletTarget
{
    homing,
    target,
}

public enum BulletColliderType
{
    noOne,
    box,
    sphrere,
}

public enum TraecrotyType
{
    straight,
}

public class BaseBullet
{
    public const float humanSize = 1f;

    public int Id;
    public float Speed;
    public float LifeTime;
    public BaseBulletTarget BaseBulletTarget;
    public Vector3 ColliderSize;
    public List<BaseEffect> Effect;
    public BulletColliderType BulletColliderType;
    public TraecrotyType TraecrotyType;
    public int MaxTargets;

    public BaseBullet(float Speed, float LifeTime, BaseBulletTarget BaseBulletTarget,
        Vector3 ColliderSize, BulletColliderType BulletColliderType, int maxTrg)
    {
        this.Speed = Speed;
        this.LifeTime = LifeTime;
        this.BaseBulletTarget = BaseBulletTarget;
        this.ColliderSize = ColliderSize;
        this.BulletColliderType = BulletColliderType;
        this.MaxTargets = maxTrg;
    }

    public BaseBullet(BaseBullet bullet1, BaseBullet bullet2,BaseSpell spell)
    {
        BulletColliderType colliderType;
        float speed = SpellMerger.MergeVal(bullet1.Speed,bullet2.Speed,true);
        float lifeTime = SpellMerger.MergeVal(bullet1.LifeTime, bullet2.LifeTime, false);
        BaseBulletTarget targetType;
//        var list = new List<BaseBulletTarget>() { bullet1.BaseBulletTarget,bullet2.BaseBulletTarget};
//        targetType = list.RandomElement();
        var colliderTypes = new List<BulletColliderType>() { bullet1.BulletColliderType, bullet2.BulletColliderType };
        colliderType = colliderTypes.RandomElement();

//        if (colliderType == BulletColliderType.noOne)
//        {
//            targetType = BaseBulletTarget.homing;
//        }
//        if (targetType == BaseBulletTarget.homing)
//        {
//            
//        }
        if (spell.TargetType == SpellTargetType.LookDirection)
        {
            targetType = BaseBulletTarget.target;
        }
        else
        {
            targetType = BaseBulletTarget.homing;
        }

        var traectoryList = new List<TraecrotyType>() { bullet1.TraecrotyType, bullet2.TraecrotyType };
        Vector3 colliderSize;
        if (colliderType == BulletColliderType.noOne)
        {
            colliderType = BulletColliderType.noOne;
            colliderSize = Vector3.zero;
        }
        else
        {
            colliderSize = MergeColliders(bullet1.ColliderSize, bullet2.ColliderSize);
        }
        TraecrotyType = traectoryList.RandomElement();
        Speed = speed;
        LifeTime = lifeTime;
        BaseBulletTarget = targetType;
        ColliderSize = colliderSize;
        BulletColliderType = colliderType;
        MaxTargets = 1;
    }

    private Vector3 MergeColliders(Vector3 s1, Vector3 s2)
    {
        Vector3 result;
        if (s1.sqrMagnitude < 0.01f || s2.sqrMagnitude < 0.01f)
        {
            result = (s1 + s2)*Random.Range(0.75f, 1.25f);
        }
        else
        {
            var xx = Random.Range(s1.x, s2.x) * 1.1f;
            var yy = Random.Range(s1.y, s2.y) * 1.1f;
            var zz = Random.Range(s1.z, s2.z) * 1.1f;
            result = new Vector3(xx,yy,zz);
        }
        return result;
    }

    public float CalcPower()
    {
        return BulletColliderType == BulletColliderType.noOne ? 1f : VectorToEffectPOwer(ColliderSize);
    }

    private static float VectorToEffectPOwer(Vector3 v)
    {
        return (v.x + v.z) / (2 * humanSize);
    }

    public string Desc(int bulletsCount)
    {
        var radius = BulletColliderType == BulletColliderType.noOne
            ? "."
            : " with " + ColliderSize.magnitude + " size. ";
        var targted = BaseBulletTarget == BaseBulletTarget.homing ? "Homing" : "Targeted";
        var bulletInfo = bulletsCount > 1 ? " bullets" : " bullet";

        string effects = Effect.Aggregate("", (current, baseEffect) => current + baseEffect.Desc());

        return targted + bulletInfo + radius +"." + effects;
        
    }
}

