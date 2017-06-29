using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class StartSpellLibrary
{
    static StartSpellLibrary()
    {
        
    }

    public static BaseSpell SimpleDamage()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2, 8, 1, 1);
        var bullet1 = new BaseBullet(0.05f, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, -160), EffectSpectials.none);

        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        return spell1;
    }

    public static BaseSpell SimplePAtck()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 3, 8, 1, 1);
        var bullet1 = new BaseBullet(1, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(15, new SubEffectData(EffectValType.percent, ParamType.PPower, 40), EffectSpectials.none);

        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        return spell1;
    }

    public static BaseSpell SimpleAOESlow()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.LookDirection, SpellCoreType.Shoot, 3,5, 1, 1);
        var bullet1 = new BaseBullet(0.07f, 0, BaseBulletTarget.target, Vector3.one*1.5f, BulletColliderType.sphrere,20);
        var effect1 = new BaseEffect(6, new SubEffectData(EffectValType.percent, ParamType.Speed, -50), EffectSpectials.none);

        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        return spell1;
    }

    public static BaseSpell MSkillBuff()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 1, 10, 1, 1);
        var bullet1 = new BaseBullet(1, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(15, new SubEffectData(EffectValType.abs, ParamType.MPower, 40), EffectSpectials.none);
        var effect2 = new BaseEffect(15, new SubEffectData(EffectValType.abs, ParamType.MDef, 40), EffectSpectials.none);

        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1, effect2 };
        return spell1;
    }

    public static BaseSpell TotemHealer()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 2, 16, 1, 1);
        var bullet1 = new BaseBullet(0.09f, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, 91), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Summon;
        spell1.BaseSummon = new BaseSummon(0.9f, 3);
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        return spell1;
    }

    public static BaseSpell TotemPDef()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2, 16, 1, 1);
        var bullet1 = new BaseBullet(0.09f, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(6, new SubEffectData(EffectValType.percent, ParamType.PDef, -50), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Summon;
        spell1.BaseSummon = new BaseSummon(0.6f, 2);
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        return spell1;
    }

    public static BaseSpell TotemSlower()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2, 16, 1, 1);
        var bullet1 = new BaseBullet(0.0001f, 10, BaseBulletTarget.homing, Vector3.one*3, BulletColliderType.sphrere, 1);
        var effect1 = new BaseEffect(6, new SubEffectData(EffectValType.percent, ParamType.PDef, -50), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Summon;
        spell1.BaseSummon = new BaseSummon(0.6f, 1);
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        return spell1;
    }

    public static BaseSpell TriggerSplitShot()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2, 15, 3, 1);
        var bullet1 = new BaseBullet(0.06f, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, -30), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Trigger;
        spell1.BaseTrigger = new BaseTrigger(1, SpellTriggerType.getDamage);
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        return spell1;
    }

    public static BaseSpell TriggerMAttack()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 2, 15, 1, 1);
        var bullet1 = new BaseBullet(1, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(10, new SubEffectData(EffectValType.abs, ParamType.MPower, 30), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Trigger;
        spell1.BaseTrigger = new BaseTrigger(2, SpellTriggerType.deathNear);
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        return spell1;
    }

    public static BaseSpell TriggerDef()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 1, 20, 1, 1);
        var bullet1 = new BaseBullet(1, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(15, new SubEffectData(EffectValType.abs, ParamType.PDef, 12), EffectSpectials.none);
        var effect2 = new BaseEffect(15, new SubEffectData(EffectValType.abs, ParamType.MDef, 12), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Trigger;
        spell1.BaseTrigger = new BaseTrigger(2, SpellTriggerType.getGold);
        bullet1.Effect = new List<BaseEffect>() { effect1, effect2 };
        return spell1;
    }
}

