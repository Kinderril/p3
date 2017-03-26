using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class EffectCreator
{

    public List<BaseEffectAbsorber> BulletHit;
    public List<BaseEffectAbsorber> BulletTrail;
    public List<BaseEffectAbsorber> CastEffect;
    public List<BaseEffectAbsorber> DurationEffect;

    public int GetEffect(BaseEffect effect)
    {
        if (effect.Duration > 0)
        {
            var e = DurationEffect.RandomElement();
            return DurationEffect.IndexOf(e);
        }
        else
        {
            var e = BulletHit.RandomElement();
            return BulletHit.IndexOf(e);
        }
    }

    public int GetEffect(BaseBullet bullet)
    {
        var e = BulletTrail.RandomElement();
        return BulletTrail.IndexOf(e);
    }

    public int GetEffect(BaseSpell spell)
    {
        var e = CastEffect.RandomElement();
        return CastEffect.IndexOf(e);
    }
}

