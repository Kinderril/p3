using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CurveAbsorber : BaseEffectAbsorber
{
    public NcCurveAnimation uvAnimation;
    public override void Play()
    {
        uvAnimation.Play();
    }

    public override void Stop()
    {

    }
}

