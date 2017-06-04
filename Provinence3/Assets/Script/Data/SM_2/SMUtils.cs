using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

public enum ParamType
{
    PPower,
    MPower,
    PDef,
    MDef,
    Heath,
    Speed,

}

public static class SMUtils
{
    public const char DELEM_VECTOR = '>';
    public const char DELEM = '|';
    public const char DELEM_BULLET = ']';
    public const char DELEM_EFFECT = ']';
    public const char DELEM_COUNT = '-';

    static Random _r = new Random((int)DateTime.Now.Ticks);

    public static float Range(float a1,float a2)
    {
        var rr =  (float)_r.NextDouble();
        float min;
        float max;
        if (a1 < a2)
        {
            min = a1;
            max = a2;
        }
        else
        {
            min = a2;
            max = a1;
        }

        return min + (max - min) *rr;
    }

    public static int Range(int a1,int a2)
    {
        return _r.Next(a1, a2);
    }

    public static float Abs(float power2)
    {
        if (power2 < 0)
        {
            return -power2;
        }
        return power2;
    }

    public static string Vector2String(Vector3 v)
    {
        return v.x.ToString() + DELEM_VECTOR + v.y.ToString() + DELEM_VECTOR + v.z.ToString();
    }

    public static Vector3 String2Vector(string info)
    {
        var ss = info.Split(DELEM_VECTOR);
        var xx = Convert.ToSingle(ss[0]);
        var yy = Convert.ToSingle(ss[1]);
        var zz = Convert.ToSingle(ss[2]);
        return new Vector3(xx,yy,zz);
    }

}

