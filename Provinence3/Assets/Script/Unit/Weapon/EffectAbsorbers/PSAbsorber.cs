using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PSAbsorber : BaseEffectAbsorber
{
    public ParticleSystem PSystem;

    void Awake()
    {
        if (PSystem == null)
        {
            PSystem = GetComponent<ParticleSystem>();
        }
        if (PSystem != null)
        {
            PSystem.Stop();
        }
    }
    public override void Play()
    {
        PSystem.Play();
    }

    public override void Stop()
    {
        PSystem.Stop();
    }
}

