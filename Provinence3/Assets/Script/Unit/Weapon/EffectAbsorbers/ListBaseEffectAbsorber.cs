using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ListBaseEffectAbsorber : BaseEffectAbsorber
{
    public List<BaseEffectAbsorber> list = new List<BaseEffectAbsorber>();
    public override void Play()
    {
        foreach (var absorber in list)
        {
            absorber.gameObject.SetActive(true);
            absorber.Play();
        }
    }

    public override void Stop()
    {
        foreach (var absorber in list)
        {
            absorber.Stop();
        }
    }
}

