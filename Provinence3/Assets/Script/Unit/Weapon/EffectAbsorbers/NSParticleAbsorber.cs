using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class NSParticleAbsorber : BaseEffectAbsorber
{
    public NcParticleSystem uvAnimation;

    void Awake()
    {
        if (uvAnimation == null)
        {
            uvAnimation = GetComponent<NcParticleSystem>();
            if (uvAnimation == null)
            {
                Debug.LogError("Some effect absorber have null effect");
            }
        }
    }

    public override void Play()
    {

        uvAnimation.gameObject.SetActive(true);
        uvAnimation.Play();
    }

    public override void Stop()
    {
        if (uvAnimation != null)
            uvAnimation.Stop();
    }
}

