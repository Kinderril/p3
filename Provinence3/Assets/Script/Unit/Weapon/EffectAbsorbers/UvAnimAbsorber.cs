using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UvAnimAbsorber : BaseEffectAbsorber
{
    public NcUvAnimation _uvAnimation;
    private Material Material;

    void Awake()
    {
        var mesh = GetComponent<MeshRenderer>();
        if (mesh != null)
        {
            Material = mesh.material;
        }
    }

    public override void Play()
    {
        _uvAnimation.gameObject.SetActive(true);
        _uvAnimation.Play();
    }

    public override void Stop()
    {
        _uvAnimation.gameObject.SetActive(false);
    }

    public override void SetColor(Color color)
    {
        if (Material != null)
            Material.SetColor("_TintColor",color);
    }
}

