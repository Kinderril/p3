using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpellMergeTestes 
{
    public SpellMergeTestes()
    {
        Init();
    }
    

    public void Init()
    {
        TestRndSpells();
//        TestSpellStart();
//        TestMerge();
    }
    
    private void LoadDB()
    {

        SpellsDataBase.LoadStartSpells(false);
        foreach (var value in SpellsDataBase.Spells.Values)
        {
            LogSpell(value);
        }
    }

    private void CreateRndSpell()
    {

//        var rndSpell = SpellsDataBase.CreateRndSpell(2);
//        Console.WriteLine("--------------");
//        Console.WriteLine("--------------");
//        LogSpell(rndSpell);
    }

    private void TestRndSpells()
    {
        LoadDB();
        Console.WriteLine("<<<<<<-------------->>>>>");
        CreateRndSpell();
        CreateRndSpell();
        CreateRndSpell();
        CreateRndSpell();
        CreateRndSpell();
        CreateRndSpell();
        CreateRndSpell();
        CreateRndSpell();
        CreateRndSpell();
        CreateRndSpell();
    }

    private void TestMerge()
    {

        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 2, 20, 1, 3);
        var bullet1 = new BaseBullet(1f, 0, BaseBulletTarget.homing, new Vector3(1, 1, 1), BulletColliderType.sphrere, 1);
        var effect1 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.PPower, 9), EffectSpectials.none);
        var summon = new BaseSummon(4f, 3);
        spell1.Bullet = bullet1;
        spell1.SpellCoreType = SpellCoreType.Summon;
        spell1.BaseSummon = summon;
        bullet1.Effect = new List<BaseEffect>() { effect1 };

        var spell2 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2, 20, 1, 3);
        var bullet2 = new BaseBullet(0.3f, 0, BaseBulletTarget.target, Vector3.zero, BulletColliderType.noOne, 2);
        var effect2 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, -76), EffectSpectials.none);
        var effect3 = new BaseEffect(10, new SubEffectData(EffectValType.percent, ParamType.PPower, -10), EffectSpectials.charging);
        spell2.Bullet = bullet2;
        bullet2.Effect = new List<BaseEffect>() { effect2, effect3 };

        LogSpell(spell1, "1");
        LogSpell(spell2, "2");
        var result = SpellMerger.Merge(spell1, spell2);
        Console.WriteLine("----");

        LogSpell(result, "result");
        LogSpell(SpellMerger.Merge(spell1, spell2), "result2");
        LogSpell(SpellMerger.Merge(spell1, spell2), "result3");
        Console.WriteLine("--------StartPowers------");

        SpellsDataBase.LoadStartSpells(false);
        foreach (var value in SpellsDataBase.Spells.Values)
        {
            LogSpell(value);
        }
    }



    private void LogSpell(BaseSpell spell,string txtFirst = "")
    {
        var txt = txtFirst +" >>>" + SpellMerger.GetPowerCoef(spell).ToString("0.0") +"<<<   " + spell.Desc();
        Console.WriteLine(txt);
//        Debug.Log(txt);
//        Debug.Log("bullet: " + spell.Bullet.Desc());
//        foreach (var baseEffect in spell.Bullet.Effect)
//        {
//            Debug.Log("effect: " + baseEffect.ToString());
//        }
    }
}

