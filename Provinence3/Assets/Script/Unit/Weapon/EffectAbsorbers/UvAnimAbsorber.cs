using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UvAnimAbsorber : BaseEffectAbsorber
{
    public NcUvAnimation _uvAnimation;
    public Material Material;
    public override void Play()
    {
        _uvAnimation.Play();
    }

    public override void Stop()
    {

    }

    public override void SetColor(Color color)
    {
        if (Material != null)
            Material.SetColor("TintColor",color);
    }
}

