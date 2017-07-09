using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Bullet))]
public class EffectBehaviour : MonoBehaviour
{
    public float Duration;
    public ParamType ParamType;
    public float Value;
    public EffectValType EffectValType;
    public Bullet Bullet { get; private set; }

    void Awake()
    {
        var bullet = GetComponent<Bullet>();
        if (bullet.AdditionaBehaviours == null)
        {
            bullet.AdditionaBehaviours = new List<EffectBehaviour>();
        }
        bullet.AdditionaBehaviours.Add(this);
        Bullet = bullet;
    }
}

