using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public  class NSBillAbsorber : BaseEffectAbsorber
{
    public NcBillboard NcBillboard;
    public override void Play()
    {
        NcBillboard.gameObject.SetActive(true);
        NcBillboard.Play();
    }

    public override void Stop()
    {
        if (NcBillboard != null)
            NcBillboard.Stop();
    }
}
