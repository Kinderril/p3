using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MeshAbsorber : BaseEffectAbsorber
{
    public GameObject obj;
    public override void Play()
    {
        obj.SetActive(true);
    }

    public override void Stop()
    {
        obj.SetActive(false);

    }
}

