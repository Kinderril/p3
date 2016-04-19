using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class StayAction : BaseAction
{
    private TimerManager.ITimer timer;
    public StayAction(BaseMonster owner, Action<bool> endCallback) 
        : base(owner, endCallback)
    {
        timer = MainController.Instance.TimerManager.MakeTimer(TimeSpan.FromSeconds(UnityEngine.Random.Range(2, 10)));
        timer.OnTimer += () =>
        {
            endCallback(false);
        };
    }

    public override void End(string msg = " end action ")
    {
        if (timer != null)
        {
            timer.Stop(owner != null && !owner.IsDead);
        }
        base.End(msg);
    }
}

