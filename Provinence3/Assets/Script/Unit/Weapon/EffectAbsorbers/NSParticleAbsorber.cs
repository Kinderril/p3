using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class NSParticleAbsorber : BaseEffectAbsorber
{
    public NcParticleSystem uvAnimation;
    public override void Play()
    {
        uvAnimation.gameObject.SetActive(true);
        uvAnimation.Play();
    }

    public override void Stop()
    {
        if (uvAnimation != null)
            uvAnimation.gameObject.SetActive(false);
    }
}

