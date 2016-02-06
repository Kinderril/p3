using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UvAnimAbsorber : BaseEffectAbsorber
{
    public NcUvAnimation _uvAnimation;
    public override void Play()
    {
        _uvAnimation.Play();
    }

    public override void Stop()
    {

    }
}

