using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpellMergeTestes
{
    public void Init()
    {
        var spell1 = new BaseSpell(SpellTargetType.Self, SpellTargetType.ClosestsEnemy, SpellCoreType.Shoot, 2,20);
        var bullet1 = new BaseBullet(1f,0,BaseBulletTarget.target, new Vector3(1,1,1), BulletColliderType.sphrere, 1);
        var effect1 = new BaseEffect(0,new SubEffectData(EffectValType.percent, ParamType.Heath, -50),EffectSpectials.none );
        spell1.Bullet = bullet1;
        bullet1.Effect = new List<BaseEffect>() {effect1};

        var spell2 = new BaseSpell(SpellTargetType.Self, SpellTargetType.Self, SpellCoreType.Shoot, 2,20);
        var bullet2 = new BaseBullet(0.3f, 0,  BaseBulletTarget.target, Vector3.zero, BulletColliderType.noOne, 1);
        var effect2 = new BaseEffect(0, new SubEffectData(EffectValType.abs, ParamType.Heath, 50), EffectSpectials.none);
        spell2.Bullet = bullet2;
        bullet2.Effect = new List<BaseEffect>() { effect2 };

        var result = SpellMerger.Merge(spell1,spell2);
        LogSpell(spell1);
        LogSpell(spell2);
        LogSpell(result);
    }

    private static void LogSpell(BaseSpell spell)
    {
        Debug.Log("spell: " + spell.ToString());
        Debug.Log("bullet: " + spell.Bullet.ToString());
        foreach (var baseEffect in spell.Bullet.Effect)
        {
            Debug.Log("effect: " + baseEffect.ToString());
        }
    }
}

