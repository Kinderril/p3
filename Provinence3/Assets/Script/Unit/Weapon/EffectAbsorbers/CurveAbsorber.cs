﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CurveAbsorber : BaseEffectAbsorber
{
    public NcCurveAnimation uvAnimation;
    public override void Play()
    {
        uvAnimation.gameObject.SetActive(true);
        uvAnimation.Play();
    }

    public override void Stop()
    {
        uvAnimation.gameObject.SetActive(false);
    }
}

