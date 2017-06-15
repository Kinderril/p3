using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class SpellsDataBase
{
    public const char MDEL = ':';

    public static Dictionary<int,BaseSpell> Spells = new Dictionary<int, BaseSpell>(); 
    public static Dictionary<int,BaseSummon> Summons = new Dictionary<int, BaseSummon>(); 
    public static Dictionary<int,BaseTrigger> Triggers = new Dictionary<int, BaseTrigger>(); 
    public static Dictionary<int,BaseEffect> Effects = new Dictionary<int, BaseEffect>(); 
    public static Dictionary<int,BaseBullet> Bullets = new Dictionary<int, BaseBullet>();

    private const string SPELLS_PREF = "SPELLS_PREF"; 
    private const string SUMMON_PREF = "SUMMON_PREF"; 
    private const string EFFECTS_PREF = "EFFECTS_PREF"; 
    private const string BULLETS_PREF = "BULLETS_PREF"; 
    private const string TRIGGERS_PREF = "TRIGGERS_PREF"; 

    public static void Add(BaseBullet data)
    {
        if (Bullets.ContainsKey(data.Id))
        {
            Bullets[data.Id] = data;
        }
        else
        {
            Bullets.Add(data.Id, data);
        }
    }
    public static void Add(BaseSummon data)
    {
        if (Summons.ContainsKey(data.Id))
        {
            Summons[data.Id] = data;
        }
        else
        {
            Summons.Add(data.Id, data);
        }
    }
    public static void Add(BaseSpell data)
    {
        if (Spells.ContainsKey(data.Id))
        {
            Spells[data.Id] = data;
        }
        else
        {
            Spells.Add(data.Id, data);
        }

    }
    public static void Add(BaseEffect data)
    {
        if (Effects.ContainsKey(data.Id))
        {
            Effects[data.Id] = data;
        }
        else
        {
            Effects.Add(data.Id, data);
        }
    }

    public static void LoadStartSpells(bool withSave = true)
    {
        if (Spells.Count <= 0)
        {
            CreateSpell(TestSpellSimpleStrike);
            CreateSpell(TestPercentStrike);
            CreateSpell(TestPercentTriggerStrike);
            CreateSpell(TestSpellPowerTotemStrike);
            CreateSpell(TestSpellPowerSplitShot);
            CreateSpell(TestSpellPowerAOE);
            CreateSpell(TestSpellArmor);
            CreateSpell(TestSpellPowerChain);
            CreateSpell(TestSpellPower);
            CreateSpell(TestSpellFlameStrike);
            CreateSpell(TestSpellPowerHeal);
            if (withSave)
                SaveDataBase();
        }
    }

    private static float PowerSpellFromLvl(int lvl)
    {
        return lvl*70 + 190;
    }

    private static void CreateSpell(Func<BaseSpell> action,int lvl = 1)
    {
        var spell = action();
        VisualEffectSetter.Set(spell);
        var power = PowerSpellFromLvl(lvl);
        SpellMerger.CalcEffectResultPower(power, spell);
        SaveSpell(spell);
    } 

    private static string SaveSpell(BaseSpell spell)
    {
        var spellID = Spells.Count;
        var bulletID = Bullets.Count;
        spell.Id = spellID;
        Add(spell);
        spell.Bullet.Id = bulletID;
        Add(spell.Bullet);
        if (spell.BaseSummon != null)
        {
            var summonId = Summons.Count;
            spell.BaseSummon.Id = summonId;
            Add(spell.BaseSummon);
        }
        foreach (var baseEffect in spell.Bullet.Effect)
        {
            var effectId = Effects.Count;
            baseEffect.Id = effectId;
            Add(baseEffect);
        }
        return spell.Save();
    }

    public static void SaveDataBase()
    {
        string saveSpell = "";
        foreach (var baseSpell in Spells)
        {
            saveSpell += baseSpell.Value.Save() + MDEL;
        }
        PlayerPrefs.SetString(SPELLS_PREF,saveSpell);
        string saveBullet = "";
        foreach (var baseSpell in Bullets)
        {
            saveBullet += baseSpell.Value.Save() + MDEL;
        }
        PlayerPrefs.SetString(BULLETS_PREF, saveBullet);
        string saveTotem = "";
        foreach (var baseSpell in Summons)
        {
            saveTotem += baseSpell.Value.Save() + MDEL;
        }
        PlayerPrefs.SetString(SUMMON_PREF, saveTotem);
        string saveEffect = "";
        foreach (var baseSpell in Effects)
        {
            saveEffect += baseSpell.Value.Save() + MDEL;
        }
        PlayerPrefs.SetString(EFFECTS_PREF, saveEffect);
        string saveTrigger = "";
        foreach (var trigger in Triggers)
        {
            saveTrigger += trigger.Value.Save() + MDEL;
        }
        PlayerPrefs.SetString(TRIGGERS_PREF, saveTrigger);
    }
    public static void LoadDataBase()
    {

        var saveSpell = PlayerPrefs.GetString(SPELLS_PREF).Split(MDEL);
        var saveBullet = PlayerPrefs.GetString(BULLETS_PREF).Split(MDEL);
        var saveTotem = PlayerPrefs.GetString(SUMMON_PREF).Split(MDEL);
        var saveEffect = PlayerPrefs.GetString(EFFECTS_PREF).Split(MDEL);
        var saveTriggers = PlayerPrefs.GetString(TRIGGERS_PREF).Split(MDEL);
        foreach (var s in saveEffect)
        {
            if (s.Length > 5)
            {
                var effect = BaseEffect.Load(s);
                Effects.Add(effect.Id,effect);
            }
        }
        foreach (var s in saveTriggers)
        {
            if (s.Length > 5)
            {
                var trigger = BaseTrigger.Load(s);
                Triggers.Add(trigger.Id,trigger);
            }
        }

        foreach (var s in saveBullet)
        {
            if (s.Length > 5)
            {
                var bullet = BaseBullet.Load(s);
                Bullets.Add(bullet.Id, bullet);
            }
        }
        foreach (var s in saveTotem)
        {
            if (s.Length > 5)
            {
                var summon = BaseSummon.Load(s);
                Summons.Add(summon.Id, summon);
            }
        }
        foreach (var s in saveSpell)
        {
            if (s.Length > 5)
            {
                var spell = BaseSpell.Load(s);
                Spells.Add(spell.Id, spell);
            }
        }
        LoadStartSpells();
    }

#region StartSpells
    private static BaseSpell TestSpellPowerHeal()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 2, 8, 1, 1);
        var bullet1 = new BaseBullet(1f, 0, BaseBulletTarget.homing, new Vector3(1, 1, 1), BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, 160), EffectSpectials.none);
        
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
//        LogSpell(spell1, "Heal");
        return spell1;
    }

    private static BaseSpell TestSpellPowerTotemStrike()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 3, 22, 2, 1);
        var bullet1 = new BaseBullet(1.5f, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, -91), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Summon;
        spell1.BaseSummon = new BaseSummon(0.9f, 3);
        bullet1.Effect = new List<BaseEffect>() { effect1 };
//        LogSpell(spell1, "Strike totem");
        return spell1;
    }

    private static BaseSpell TestPercentStrike()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 3, 22, 1, 1);
        var bullet1 = new BaseBullet(0.004f, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.percent, ParamType.Heath, -35), EffectSpectials.none);
        spell1.Bullet = bullet1;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
        //        LogSpell(spell1, "Flame strike");
        return spell1;

    }

    private static BaseSpell TestSpellFlameStrike()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.LookDirection, SpellCoreType.Shoot, 3, 22, 1, 1);
        var bullet1 = new BaseBullet(1.5f, 1, BaseBulletTarget.target, Vector3.one * 1.5f, BulletColliderType.sphrere, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, -201), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
//        LogSpell(spell1, "Flame strike");
        return spell1;
    }

    private static BaseSpell TestPercentTriggerStrike()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 3, 22, 1, 1);
        var bullet1 = new BaseBullet(1.5f, 1, BaseBulletTarget.target, Vector3.zero, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.percent, ParamType.Heath, -35), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Trigger;
        spell1.BaseTrigger = new BaseTrigger(3,SpellTriggerType.shootMagic);
        bullet1.Effect = new List<BaseEffect>() { effect1 };
//        LogSpell(spell1, "Flame strike");
        return spell1;
    }

    private static BaseSpell TestSpellSimpleStrike()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2, 21, 1, 1);
        var bullet1 = new BaseBullet(0.002f, 0, BaseBulletTarget.homing, Vector3.one * 1.5f, BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, -241), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
//        LogSpell(spell1, "Simple strike");
        return spell1;
    }

    private static BaseSpell TestSpellPowerChain()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2, 22, 1, 1);
        var bullet1 = new BaseBullet(0f, 0, BaseBulletTarget.homing, Vector3.zero, BulletColliderType.noOne, 4);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, -80), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
//        LogSpell(spell1, "Chain light");
        return spell1;
    }

    private static BaseSpell TestSpellPowerSplitShot()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2, 15, 7, 1);
        var bullet1 = new BaseBullet(1f, 0, BaseBulletTarget.homing, new Vector3(1, 1, 1), BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, -30), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Trigger;
        spell1.BaseTrigger = new BaseTrigger(1, SpellTriggerType.getDamage);
        bullet1.Effect = new List<BaseEffect>() { effect1 };
//        LogSpell(spell1, "Split shot");
        return spell1;
    }

    private static BaseSpell TestSpellPowerAOE()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 1, 19, 1, 1);
        var bullet1 = new BaseBullet(1f, 0, BaseBulletTarget.homing, new Vector3(4, 4, 4), BulletColliderType.sphrere, 1);
        var effect1 = new BaseEffect(7, new SubEffectData(EffectValType.abs, ParamType.Heath, -80), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1 };
//        LogSpell(spell1, "AOE");
        return spell1;
    }

    private static BaseSpell TestSpellArmor()
    {
        var time = 12f;
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 1, 19, 1, 1);
        var bullet1 = new BaseBullet(1f, 0, BaseBulletTarget.homing, new Vector3(4, 4, 4), BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(time, new SubEffectData(EffectValType.percent, ParamType.PDef, 50), EffectSpectials.none);
        var effect2 = new BaseEffect(time, new SubEffectData(EffectValType.percent, ParamType.MDef, 50), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Shoot;
        bullet1.Effect = new List<BaseEffect>() { effect1, effect2 };
//        LogSpell(spell1, "Armor");
        return spell1;
    }

    private static BaseSpell TestSpellPower()
    {
        var time = 12f;
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 1, 19, 1, 1);
        var bullet1 = new BaseBullet(1f, 0, BaseBulletTarget.homing, new Vector3(4, 4, 4), BulletColliderType.noOne, 1);
        var effect1 = new BaseEffect(time, new SubEffectData(EffectValType.percent, ParamType.PPower, 70), EffectSpectials.none);
        //        var effect2 = new BaseEffect(time, new SubEffectData(EffectValType.percent, ParamType.MPower, 50), EffectSpectials.none);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Trigger;
        spell1.BaseTrigger = new BaseTrigger(2,SpellTriggerType.getGold);
        bullet1.Effect = new List<BaseEffect>()
        {
            effect1
        };
//        LogSpell(spell1, "Power");
        return spell1;
    }
    #endregion
}

