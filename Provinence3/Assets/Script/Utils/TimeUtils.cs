
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;

public class TimeUtils
{
    private static Stopwatch stopwatch;
    private static string measureName = "";
    private static Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();
    public static void Init()
    {
        stopwatch = new Stopwatch();
    }

    public static void StartMeasure(string name)
    {
        if (stopwatch != null)
        {
            measureName = name;
            stopwatch.Reset();
            stopwatch.Start();
            UnityEngine.Debug.Log( "StartMeasure " + " " + measureName + " :" + 0 + " ms.");
        }
        else
        {
            Init();
            StartMeasure(name);
        }
        if (dict.ContainsKey(name))
        {
            UnityEngine.Debug.Log("EndMeasure " + " " + name + " :" + (System.DateTime.Now - dict[name]).TotalMilliseconds + " ms.");
        }
        dict[name] = DateTime.Now;
    }

    public static long EndMeasure()
    {
        if (stopwatch != null)
        {
            long elapsed = stopwatch.Elapsed.Ticks / TimeSpan.TicksPerMillisecond;
            stopwatch.Stop();
            UnityEngine.Debug.Log( "EndMeasure " + " " + measureName + " :" + elapsed + " ms.");
            return elapsed;
        }
        return 0;
    }

    public static string EndMeasure(string name)
    {
        double ms = 0;
        if (dict.ContainsKey(name))
        {
            ms = (DateTime.Now - dict[name]).TotalMilliseconds;
            var str = "EndMeasure " + " " + name + " :" + ms + " ms.";
            UnityEngine.Debug.Log(str);
            dict[name] = DateTime.Now;
        }
        return name + " " + ms + " \n";
    }
}

