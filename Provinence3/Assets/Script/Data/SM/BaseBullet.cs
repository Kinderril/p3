using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;

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
    private const char DELEM_EFFECT = ']';

    public const float HUMAN_SIZE = 1f;
    public const float MAX_TRG_COEF = 0.65f;

    public int Id;
    public int IdVisual;
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
        if (spell.TargetType == SpellTargetType.Self)
        {
            MaxTargets = 1;
        }
        else
        {
            MaxTargets = SMUtils.Range(bullet1.MaxTargets, bullet1.MaxTargets);
        }
    }


    private Vector3 MergeColliders(Vector3 s1, Vector3 s2)
    {
        Vector3 result;
        if (s1.sqrMagnitude < 0.01f || s2.sqrMagnitude < 0.01f)
        {
            result = (s1 + s2)*SMUtils.Range(0.75f, 1.25f);
        }
        else
        {
            var xx = SMUtils.Range(s1.x, s2.x) * 1.1f;
            var yy = SMUtils.Range(s1.y, s2.y) * 1.1f;
            var zz = SMUtils.Range(s1.z, s2.z) * 1.1f;
            result = new Vector3(xx,yy,zz);
        }
        return result;
    }

    public float CalcPower()
    {
        var trg = 1f;
        if (BaseBulletTarget == BaseBulletTarget.homing)
        {
            trg = 1.3f;
        }
        else
        {
            trg = 1f;
        }
        var collider = BulletColliderType == BulletColliderType.noOne ? 1f : VectorToEffectPOwer(ColliderSize);
        var maxTrg = (float)MaxTargets * MAX_TRG_COEF;
        return collider*maxTrg * trg;
    }

    private static float VectorToEffectPOwer(Vector3 v)
    {
        return (v.x + v.z) / (2 * HUMAN_SIZE);
    }

    public string Desc(BaseSpell spell)
    {
        var radius = BulletColliderType == BulletColliderType.noOne
            ? "."
            : " with AOE.";

        var bulletInfo = spell.BulletCount > 1 ? " " + spell.BulletCount + " bullets" : " single bullet";
        string effects = "";
        for (int i = 0; i < Effect.Count; i++)
        {
            var effect = Effect[i];
            string ss = effect.Desc(spell) + "\n";
            effects += ss;

        }
        return "Use" + bulletInfo + radius + " \n Effects:" + effects;
    }

    public string DescFull(BaseSpell spell)
    {
        var radius = BulletColliderType == BulletColliderType.noOne
            ? "."
            : " with " + ColliderSize.magnitude.ToString("0.0") + " size. ";
        var targted = BaseBulletTarget == BaseBulletTarget.homing ? "Homing" : "Targeted";
        var bulletInfo = spell.BulletCount > 1 ? " " + spell.BulletCount + " bullets" : " bullet";
        var mt = " max targets:" + MaxTargets;
        string effects = Effect.Aggregate("", (current, baseEffect) => current + " {" + baseEffect.DescFull(spell) +"}");
        return targted + bulletInfo + radius + mt + "." + effects;
    }

    public string Save()
    {
//        var result = "";
        var baseBullet = Id.ToString() + SMUtils.DELEM_BULLET + //0
                         IdVisual.ToString() + SMUtils.DELEM_BULLET +  //1
                         Speed.ToString() + SMUtils.DELEM_BULLET + //2
                         LifeTime + SMUtils.DELEM_BULLET + //3
                         ((int)BaseBulletTarget).ToString() + SMUtils.DELEM_BULLET + //4
                         SMUtils.Vector2String(ColliderSize) + SMUtils.DELEM_BULLET + //5
                         ((int)BulletColliderType).ToString() + SMUtils.DELEM_BULLET + //6
                         ((int)TraecrotyType).ToString() + SMUtils.DELEM_BULLET + //7
                         MaxTargets;                     //8
        string effets = "";//9
        foreach (var baseEffect in Effect)
        {
            effets += baseEffect.Id.ToString() + SMUtils.DELEM;
        }
        baseBullet += SMUtils.DELEM_BULLET + effets;
        return baseBullet;
    }

    public static BaseBullet Load(string info)
    {
        var ss = info.Split(SMUtils.DELEM_BULLET);
        int id = Convert.ToInt32(ss[0]);
        int IdVisual = Convert.ToInt32(ss[1]);
        float Speed = Convert.ToSingle(ss[2]);
        float LifeTime = Convert.ToSingle(ss[3]);
        BaseBulletTarget BaseBulletTarget = (BaseBulletTarget)Enum.Parse(typeof(BaseBulletTarget), ss[4]);
        Vector3 v = SMUtils.String2Vector(ss[5]);
        BulletColliderType BulletColliderType = (BulletColliderType)Enum.Parse(typeof(BulletColliderType), ss[6]);
        TraecrotyType TraecrotyType = (TraecrotyType)Enum.Parse(typeof(TraecrotyType), ss[7]);
        int MaxTargets = Convert.ToInt32(ss[8]);
        var ssEffects = ss[9].Split(SMUtils.DELEM);
        List<BaseEffect> effects = new List<BaseEffect>();
        foreach (var ssEffect in ssEffects)
        {
            if (ssEffect.Length > 0)
            {
                var idE = Convert.ToInt32(ssEffect);
                effects.Add(SpellsDataBase.Effects[idE]);
            }
        }
        BaseBullet bullet = new BaseBullet(Speed,LifeTime,BaseBulletTarget,v,BulletColliderType, MaxTargets);
        bullet.Effect = effects;
        bullet.Id = id;
        bullet.IdVisual = IdVisual;
        bullet.TraecrotyType = TraecrotyType;
        return bullet;
    }
}

