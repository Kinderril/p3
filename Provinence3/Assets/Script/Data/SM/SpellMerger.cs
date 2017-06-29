using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;

//using UnityEngine;

public enum EffectPositiveType
{
    Positive,
    Negative,
//    Both,
}

public enum TMpModifType
{
    none,

    addSpecial,//Добавляем рандомный спешиал из заранее созданого спеска // OK
    shoot,//Обязательно не суммонер //ОК
    summon,//Обязательно сумонер //ОК
    trigger,//Обязательно триггер //ОК
    moreRandom, //Увеличиваем разброс//ОК
}

public static class SpellMerger
{
    public static List<EffectSpectials> Specials2Rnd = new List<EffectSpectials>()
    {
        EffectSpectials.charging,EffectSpectials.dependsOnDist,
//        EffectSpectials.heroMPower,
//        EffectSpectials.hpOfSelf,
        EffectSpectials.hpOfTarget
    }; 

    public static List<SpellTriggerType> Triggers2Rnd = new List<SpellTriggerType>()
    {
        SpellTriggerType.shoot,
        SpellTriggerType.shootMagic,
        SpellTriggerType.getDamage,
        SpellTriggerType.cast,
        SpellTriggerType.deathNear,
        SpellTriggerType.getGold,
        SpellTriggerType.questAction,
        SpellTriggerType.takeBonus,
        SpellTriggerType.getEffect,
    }; 

    public const float BASE_COST = 20f;
    public const float COST_COEF = 0.06f;
    public const float CHARGE_COEF = 0.15f;
    public const float DIFF_COST_CHARGE = 0.4f;
    public const float MORE_RND_COEF = 1.6f;


    public static BaseSpell Merge(BaseSpell spell1, BaseSpell spell2,float powerCoef = 0f, TMpModifType modifType= TMpModifType.none)
    {
        //Считаем среднюю мощность заклинания.
        var power1 = GetPowerCoef(spell1);
        var power2 = GetPowerCoef(spell2);
        var rangeResult = SMUtils.Range(SMUtils.Abs(power2), SMUtils.Abs(power1));
        var rndPowerTotal = rangeResult * (1.05f + powerCoef);
//        Console.WriteLine("Power of new spell:" + rndPowerTotal + " <<<<< " + rangeResult);
//        return null;
        //Начинаем мержить базу для заклинания
        int level = SMUtils.Range(spell1.Level, spell1.Level + 1);
        var listStartTrg = new List<SpellTargetType>() {spell1.StartType,spell2.StartType};
        var start = listStartTrg.RandomElement();

        var listTrgEnd = new List<SpellTargetType>() { spell1.TargetType, spell2.TargetType };
        var end = listTrgEnd.RandomElement();

        //        bool isPositiveSpell = end == SpellTargetType.Self;

        var core = (new List<SpellCoreType>() { spell1.SpellCoreType, spell2.SpellCoreType }).RandomElement();
        switch (modifType)
        {
            case TMpModifType.shoot:
                core = SpellCoreType.Shoot;
                break;
            case TMpModifType.summon:
                core = SpellCoreType.Summon;
                break;
            case TMpModifType.trigger:
                core = SpellCoreType.Trigger;
                break;
        }
        switch (core)
        {
            case SpellCoreType.Shoot:
                break;
            case SpellCoreType.Summon://Если это саммон то мы стреляем всегда из себя (В реализации из тотема)
                start = SpellTargetType.Self;
                if (end == SpellTargetType.LookDirection)
                    end = SpellTargetType.ClosestsEnemy;
                break;
            case SpellCoreType.Trigger:
                //No restrictions
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        var d = DIFF_COST_CHARGE;
        if (modifType == TMpModifType.moreRandom)
        {
            d *= MORE_RND_COEF;
        }
        var charges = (int) Mathf.Clamp(SMUtils.Range(spell1.Charges*SMUtils.Range(d, 1f + d), spell2.Charges*SMUtils.Range(d, 1f + d)), 1, 5);
        var cost = (int) Mathf.Clamp(SMUtils.Range(spell1.Cost*SMUtils.Range(d, 1f + d), spell2.Cost*SMUtils.Range(d, 1f + d)), 10, 40);

        var bulletCnt = SMUtils.Range(spell1.BulletCount, spell2.BulletCount);

        BaseSpell resultSpell = new BaseSpell(start, end, core, charges, cost, bulletCnt, level);
        switch (core)
        {
            case SpellCoreType.Summon:
                resultSpell.BaseSummon = BaseSummon.Merge(spell1.BaseSummon, spell2.BaseSummon);
                break;
            case SpellCoreType.Trigger:
                resultSpell.BaseTrigger = BaseTrigger.Merge(spell1.BaseTrigger, spell2.BaseTrigger);
                break;
        }


        BaseBullet bullet = new BaseBullet(spell1.Bullet, spell2.Bullet, resultSpell);
        resultSpell.Bullet = bullet;

        //Если коллайдера нету и мы не самонаводящиеся пуля то все очень плохо)
        if (bullet.BulletColliderType == BulletColliderType.noOne && resultSpell.TargetType == SpellTargetType.LookDirection)
        {
            resultSpell.TargetType = SpellTargetType.ClosestsEnemy;
        }


        //В зависимости от типа пули надо посчитать каким может быть эффект
        EffectPositiveType positiveType = EffectPositiveType.Negative;

        switch (resultSpell.TargetType)
        {
            case SpellTargetType.Self:
                positiveType = EffectPositiveType.Positive;
                break;
            case SpellTargetType.ClosestsEnemy:
                positiveType = EffectPositiveType.Negative;
                break;
            case SpellTargetType.LookDirection:
                positiveType = EffectPositiveType.Negative;
                break;
        }
        rndPowerTotal = ModifByBulletAndSpell(rndPowerTotal, resultSpell); //Посчитали какая мощность будет дана на эффекты в зависимости от кол-ва пули и прочего

        var effects = spell1.Bullet.Effect.ToList();
        effects.AddRange(spell2.Bullet.Effect);
        int effectsCount = SMUtils.Range(1, spell1.Bullet.Effect.Count + spell2.Bullet.Effect.Count - 1);

        float offset = .1f;
        if (modifType == TMpModifType.moreRandom)
        {
            offset = 0.35f;
        }
        var resultEffects = new List<BaseEffect>();
        for (int i = 0; i < effectsCount; i++)
        {
            var delta = (rndPowerTotal/(float) effectsCount)*SMUtils.Range(1f - offset, 1f + offset);
            if (delta > 99999 ||Single.IsNaN(delta))
            {
                Console.WriteLine(">>>");
            }
            resultEffects.Add(BaseEffect.Create(effects, delta, resultSpell.Level, positiveType, resultSpell));
        }
        bullet.Effect = resultEffects;
        if (modifType == TMpModifType.addSpecial)
        {
            var e = resultEffects[0];
            e.Spectial = Specials2Rnd.RandomElement();
        }
        VisualEffectSetter.Set(resultSpell);
//        Console.WriteLine("Power of end spell:" + GetPowerCoef(resultSpell) + " <<<<< "  );
        return resultSpell;
    }

    public static void CalcEffectResultPower(float power, BaseSpell spell)
    {
        var rndPowerTotal = ModifByBulletAndSpell(power, spell); //Посчитали какая мощность будет дана на эффекты в зависимости от кол-ва пули и прочего
        var countEffects = spell.Bullet.Effect.Count;
        var delta = rndPowerTotal/(float)countEffects;
        List<BaseEffect> effects2set = new List<BaseEffect>();
        foreach (var baseEffect in spell.Bullet.Effect)
        {
            var e = BaseEffect.CreateWithBase(baseEffect,delta,spell.TargetType,spell.Level);
            effects2set.Add(e);
        }
        spell.Bullet.Effect = effects2set;
    }

    private static float ModifByBulletAndSpell(float rndPowerTotal, BaseSpell spell)
    {
//        var costCoef = 1f + (spell.Cost - BASE_COST)*COST_COEF;
//        var chargeCoef = 1f + Mathf.Clamp((spell.Charges - 1f)*CHARGE_COEF, 0, 2f);
        var p1 = SubPower(spell);
        var r = rndPowerTotal/p1;
        return r;
//        return rndPowerTotal /**chargeCoef*costCoef *//SubPower(spell);
    }

    public static float GetPowerCoef(BaseSpell spell)
    {
        float val = 0;
        foreach (var baseEffect in spell.Bullet.Effect)
        {
            var v = baseEffect.CalcValue(spell);
            val += v;
        }
        var p = SubPower(spell);
        return p*val;
    }

    private static float SubPower(BaseSpell spell)
    {
        var costCoef = 1f + (BASE_COST - spell.Cost)*COST_COEF;
        var chargeCoef = 1f + Mathf.Clamp((spell.Charges - 1f)*CHARGE_COEF, 0, 2f);

        BaseBullet bullet = spell.Bullet;
        if (spell.BulletCount == 0)
        {
//            Debug.LogError("Spell bullets count 0");
        }
        var cnt = spell.BulletCount;
        float summonCoef = 1f;
        float triggerCoef = 1f;
        if (spell.BaseSummon != null)
        {
            summonCoef = spell.BaseSummon.CalcPower();
        }
        if (spell.BaseTrigger != null)
        {
            triggerCoef =spell.BaseTrigger.CalcPower();
        }
        var bulletPower = bullet.CalcPower();
        var r = bulletPower * cnt * summonCoef * costCoef * chargeCoef * triggerCoef;

        return r;
    }


    public static float MergeVal(float v1, float v2, bool canbezero)
    {
        float range;
        if (v1 <= 0.001f || v2 <= 0.001f)
        {
            if (SMUtils.Range(0, 100) < 50 && canbezero)
            {
                range = 0f;
            }
            else
            {
                var spbMid = (v1 + v2)/2;
                range = SMUtils.Range(spbMid*0.75f, spbMid*1.25f);
            }
        }
        else
        {
            range = SMUtils.Range(v1, v2);
        }
        return range;
    }
}

