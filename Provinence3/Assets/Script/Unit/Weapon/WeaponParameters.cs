using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType
{
    magic,
    physics,
}

public  class WeaponParameters : ScriptableObject
{
    public const float MID_MAX_RANGE = 8;
//    public float bulletSpeed;
    public WeaponType type;
    public float range;
    public bool isHoming = false;
    public float attackCooldown;
    public Sprite icon;
}

