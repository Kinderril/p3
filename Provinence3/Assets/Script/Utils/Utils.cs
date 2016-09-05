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
    private static int groundLayerIndex;
    private static int MaxId;


    public static void SetId(int id)
    {
        if (id > MaxId)
        {
            MaxId = id;
        }
    }

    public static int GetId()
    {
        MaxId++;
        return MaxId;
    }
    public static void Init(Terrain terrain)
    {
        Utils.groundLayerIndex = 1 << terrain.gameObject.layer;
    }

    public static T RandomElement<T>(this List<T> list)
    {
        if (list.Count == 0)
            return default(T);
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    public static List<T> RandomElement<T>(this List<T> list,int count)
    {
        var listOut = new List<T>();
        if (list.Count == 0)
            return listOut;

        
        for (int i = 0; i < count; i++)
        {
            var a = (list.Count/count);
            var e = list[UnityEngine.Random.Range(i * a + 1, (i+1) * a)];
            listOut.Add(e);
        }

        return listOut;
    }

    public static void Sort<T>(List<T> list, Func<T, int> GetPriority) where T : MonoBehaviour
    {
        list.Sort((x, y) =>
        {
            var xPriority = GetPriority(x);
            var yPriority = GetPriority(y);
            if (xPriority > yPriority)
            {
                return 1;
            }

            if (yPriority > xPriority)
            {
                return -1;
            }
            return 0;
        });
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var pe = list[i];
            pe.transform.SetAsLastSibling();
        }
    }
    public static void SetRandomRotation(Transform transform)
    {
        transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0);
    }

    public static void GroundTransform(Transform transform, float checkDist = 9999f)
    {
        RaycastHit hitInfo;
        var p = new Vector3(transform.position.x, 100, transform.position.z);
        Ray ray = new Ray(p, Vector3.down);
//        Debug.DrawRay(p, Vector3.down * 100, Color.yellow, 20);

        

        if (Physics.Raycast(ray, out hitInfo, checkDist,groundLayerIndex))
        {
            var t = transform.position;
//            var groundOffset = hitInfo.distance;
            transform.position = new Vector3(t.x, hitInfo.point.y, t.z);
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


public class Taple<T1, T2>
{
    public T1 val1;
    public T2 val2;

    public Taple(T1 t1, T2 t2)
    {
        val1 = t1;
        val2 = t2;
    }

}

