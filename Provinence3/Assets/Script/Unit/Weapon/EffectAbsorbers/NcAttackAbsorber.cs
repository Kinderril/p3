using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class NcAttackAbsorber :BaseEffectAbsorber
{
    public NcAttachPrefab AttackAttachPrefab;
    public override void Play()
    {
        AttackAttachPrefab.gameObject.SetActive(true);
        AttackAttachPrefab.Play();
        base.Play();
    }

    public override void Stop()
    {
        AttackAttachPrefab.gameObject.SetActive(false);
        AttackAttachPrefab.Stop();
        base.Stop();
    }
}

