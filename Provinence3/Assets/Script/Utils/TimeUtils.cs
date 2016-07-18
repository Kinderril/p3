
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

    public static void EndMeasure(string name)
    {
        if (dict.ContainsKey(name))
        {
            UnityEngine.Debug.Log( "EndMeasure " + " " + name + " :" + (DateTime.Now - dict[name]).TotalMilliseconds + " ms.");
            dict[name] = DateTime.Now;
        }
    }
}

