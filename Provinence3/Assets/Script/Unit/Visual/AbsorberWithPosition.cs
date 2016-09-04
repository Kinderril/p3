using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AbsorberWithPosition : MonoBehaviour
{
    public VisualEffectPosition VisualEffectPosition;
    public BaseEffectAbsorber Absorber;

    public void SetAndPlay(Unit unit)
    {
        switch (VisualEffectPosition)
        {
            case VisualEffectPosition.core:
                transform.SetParent(unit.transform, false);
                break;
            case VisualEffectPosition.weapon:
                transform.SetParent(unit.weaponsContainer, false);
                break;
        }
        Absorber.Play();


    }

    public void Stop()
    {
        Absorber.Stop();
    }
}

