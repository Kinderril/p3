using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;

public enum EffectPositiveType
{
    Positive,
    Negative,
    Both,
}

public static class SpellMerger
{
    public static BaseSpell Merge(BaseSpell spell1, BaseSpell spell2)
    {
        //Считаем среднюю мощность заклинания.
        var power1 = GetPowerCoef(spell1);
        var power2 = GetPowerCoef(spell2);
        var rndPowerTotal = UnityEngine.Random.Range(Mathf.Abs(power2), Mathf.Abs(power1))*1.15f;
        
        //Начинаем мержить базу для заклинания
        int level = UnityEngine.Random.Range(spell1.Level, spell1.Level + 1);
        var listStartTrg = new List<SpellTargetType>() {spell1.StartType,spell2.StartType};
        var start = listStartTrg.RandomElement();

        var listTrg = new List<SpellTargetType>() { spell1.TargetType, spell2.TargetType };
        var end = listTrg.RandomElement();

//        bool isPositiveSpell = end == SpellTargetType.Self;
        
        var core = (new List<SpellCoreType>() { spell1.SpellCoreType, spell2.SpellCoreType }).RandomElement();
        var charges = UnityEngine.Random.Range(spell1.Charges, spell2.Charges);
        var cost = UnityEngine.Random.Range(spell1.Cost, spell2.Cost);

        BaseSpell resultSpell = new BaseSpell(start, end, core, charges, cost);
        switch (core)
        {
            case SpellCoreType.Summon:
                BaseSummon bsSummon;
                if (spell1.BaseSummon != null)
                {
                    bsSummon = spell1.BaseSummon;
                }
                else
                {
                    bsSummon = spell2.BaseSummon;
                }
                resultSpell.BaseSummon = bsSummon.CopyWithRnd();
                break;
        }


        BaseBullet bullet = new BaseBullet(spell1.Bullet, spell2.Bullet, resultSpell);
        resultSpell.Bullet = bullet;
        resultSpell.Level = level;



        //Если коллайдера нету и мы не самонаводящиеся пуля то все очень плохо)
        if (bullet.BulletColliderType == BulletColliderType.noOne &&
            resultSpell.TargetType == SpellTargetType.LookDirection)
        {
            resultSpell.TargetType = SpellTargetType.ClosestsEnemy;
        }

        //В зависимости от типа пули надо посчитать каким может быть эффект
        EffectPositiveType positiveType = EffectPositiveType.Both;
        switch (bullet.BulletColliderType)
        {
            case BulletColliderType.noOne:
                switch (resultSpell.TargetType)
                {
                    case SpellTargetType.Self:
                        positiveType = EffectPositiveType.Positive;
                        break;
                    case SpellTargetType.ClosestsEnemy:
                        positiveType = EffectPositiveType.Negative;
                        break;
                    case SpellTargetType.LookDirection:
                        //такого варианта быть не может
                        break;
                }

                break;
            case BulletColliderType.box:
            case BulletColliderType.sphrere:
                positiveType = EffectPositiveType.Both;
                break;
        }


        rndPowerTotal = ModifByBullet(rndPowerTotal, resultSpell); //Посчитали какая мощность будет дана на эффекты в зависимости от кол-ва пули и прочего
        var effects = spell1.Bullet.Effect.ToList();
        effects.AddRange(spell2.Bullet.Effect);
        int effectsCount = UnityEngine.Random.Range(1, spell1.Bullet.Effect.Count + spell2.Bullet.Effect.Count - 1);


        var resultEffects = new List<BaseEffect>();
        for (int i = 0; i < effectsCount; i++)
        {
            var delta = (rndPowerTotal/(float) effectsCount)*UnityEngine.Random.Range(0.9f, 1.1f);
            resultEffects.Add(BaseEffect.Create(effects, delta, resultSpell.Level, positiveType));
        }
        bullet.Effect = resultEffects;
        return resultSpell;
    }

    private static float ModifByBullet(float rndPowerTotal, BaseSpell spell)
    {
        return rndPowerTotal/SubPower(spell);
    }

    private static float GetPowerCoef(BaseSpell spell)
    {
        float val = 0;
        foreach (var baseEffect in spell.Bullet.Effect)
        {
            val += baseEffect.CalcValue(spell.Level);
        }
        return SubPower(spell)*val;
    }

    private static float SubPower(BaseSpell spell)
    {
        BaseBullet bullet = spell.Bullet;
        var cnt = spell.BulletCount;
        float summonCoef = 1f;
        if (spell.BaseSummon != null)
        {
            summonCoef = spell.BaseSummon.CalcPower();
        }
        return bullet.CalcPower()*cnt*summonCoef;
    }


    public static float MergeVal(float v1, float v2, bool canbezero)
    {
        float range;
        if (v1 <= 0.001f || v2 <= 0.001f)
        {
            if (UnityEngine.Random.Range(0, 100) < 50 && canbezero)
            {
                range = 0f;
            }
            else
            {
                var spbMid = (v1 + v2)/2;
                range = UnityEngine.Random.Range(spbMid*0.75f, spbMid*1.25f);
            }
        }
        else
        {
            range = UnityEngine.Random.Range(v1, v2);
        }
        return range;
    }
}

