using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Utils
{
    private static bool haveNextNextGaussian;
    private static float nextNextGaussian;
    private static readonly Random random = new Random();
    private static bool uselast = true;
    private static double next_gaussian;

    public static T RandomElement<T>(this List<T> list)
    {
        if (list.Count == 0)
            return default(T);
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    
    public static void SetRandomRotation(Transform transform)
    {
        transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0);
    }

    public static void GroundTransform(Transform transform, float checkDist = 9999f)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, checkDist))
        {
            var t = transform.position;
            var groundOffset = hitInfo.distance;
            transform.position = new Vector3(t.x, t.y - groundOffset, t.z);
        }
    }

    public static float RandomNormal(float min, float max)
    {
        var deviations = 3.5;
        double r;
        while ((r = BoxMuller(min + (max - min)/2.0, (max - min)/2.0/deviations)) > max || r < min)
        {
        }

        return (float)r;
    }

    public static double BoxMuller(double mean, double standard_deviation)
    {
        return mean + BoxMuller()*standard_deviation;
    }

    public static double BoxMuller()
    {
        if (uselast)
        {
            uselast = false;
            return next_gaussian;
        }
        double v1, v2, s;
        do
        {
            v1 = 2.0*random.NextDouble() - 1.0;
            v2 = 2.0*random.NextDouble() - 1.0;
            s = v1*v1 + v2*v2;
        } while (s >= 1.0 || s == 0);

        s = Math.Sqrt((-2.0*Math.Log(s))/s);

        next_gaussian = v2*s;
        uselast = true;
        return v1*s;
    }

    public static void ClearTransform(Transform t)
    {
        foreach (Transform v in t)
        {
            GameObject.Destroy(v.gameObject);
        }
    }

    public static void ForceAddValue<T>(this List<T> l, T v)
    {
        if (!l.Contains(v))
        {
            l.Add(v);
        }
    }
}