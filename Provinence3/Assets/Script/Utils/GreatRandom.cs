using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class GreatRandom
{
    public static float RandomizeValue(float v)
    {
        return UnityEngine.Random.Range(v*0.75f, v*1.25f);
    }

    public static int RandomizeValue(int v)
    {
        return UnityEngine.Random.Range((int)(v * 0.75f), (int)(v * 1.25f));
    }
}

