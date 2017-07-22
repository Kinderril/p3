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
    public static void Add(BaseTrigger data)
    {
        if (Triggers.ContainsKey(data.Id))
        {
            Triggers[data.Id] = data;
        }
        else
        {
            Triggers.Add(data.Id, data);
        }
    }

    public static void LoadStartSpells(bool withSave = true)
    {
        if (Spells.Count <= 0)
        {
            CreateSpell(StartSpellLibrary.SimpleDamage);
            CreateSpell(StartSpellLibrary.SimpleAOESlow);
            CreateSpell(StartSpellLibrary.SimplePAtck);
            CreateSpell(StartSpellLibrary.TotemHealer);
            CreateSpell(StartSpellLibrary.TotemPDef);
            CreateSpell(StartSpellLibrary.TotemSlower);
            CreateSpell(StartSpellLibrary.TriggerDef);
            CreateSpell(StartSpellLibrary.TriggerMAttack);
            CreateSpell(StartSpellLibrary.TriggerSplitShot);
            if (withSave)
                SaveDataBase();
        }
    }

    private static float PowerByLvl(int l)
    {
        return 190f + l*60f;
    }

//    public static BaseSpell CreateRndSpell(int lvl)
//    {
//        var spells = Spells.Values;
//        var s1 = spells.RandomElement();
//        var s2 = spells.RandomElement();
//        //to del
//        if (s1 == s2)
//        {
//            s2 = spells.RandomElement();
//        }
//        //end 2 del
//
//        var result = SpellMerger.Merge(s1, s2);
////        var power = PowerByLvl(lvl);// * SMUtils.Range(0.9f,1.1f);
////        SpellMerger.CalcEffectResultPower(power, result);
//        return result;
//    }

    private static void CreateSpell(Func<BaseSpell> action)
    {
        var spell = action();
        VisualEffectSetter.Set(spell);
        var power = PowerByLvl(1) * SMUtils.Range(0.93f, 1.07f);
        SpellMerger.CalcEffectResultPower(power, spell);
        SaveSpell(spell);
    }
    private static float PowerSpellFromLvl(int lvl)
    {
        return lvl * 70 + 190;
    }
    public static BaseSpell CreatSpellData(int level)
    {
        var list = Spells.Values.ToList();
        var rndSpells = list.RandomElement(2);
        var spell = SpellMerger.Merge(rndSpells[0], rndSpells[1]);
        SpellMerger.CalcEffectResultPower(PowerSpellFromLvl(level), spell);
        VisualEffectSetter.Set(spell);
        spell.Level = level;
        SaveSpell(spell);
        return spell;
    }

    public static string SaveSpell(BaseSpell spell)
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
        if (spell.BaseTrigger != null)
        {
            var trgId = Triggers.Values.Count;
            spell.BaseTrigger.Id = trgId;
            Add(spell.BaseTrigger);
        }
        SaveDataBase();
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
}

