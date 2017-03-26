using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTester : MonoBehaviour {

    public BaseEffectAbsorber Effect;

    public void TestDo()
    {
        if (Effect != null)
        {
            Effect.Play();
        }
    }

    public void Stop()
    {
        if (Effect != null)
        {
            Effect.Stop();
        }
    }
}
