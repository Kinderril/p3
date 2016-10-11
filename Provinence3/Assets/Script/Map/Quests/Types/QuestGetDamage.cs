using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class QuestGetDamage : QuestLogicBase
{
    public QuestGetDamage(QuestGiver QuestGiver, int targetCount,  Action<int, int> OnQuestProgressChange) : base(QuestGiver, targetCount, OnQuestProgressChange)
    {
        MainController.Instance.level.MainHero.OnGetHit += OnGetHit;
    }

    private void OnGetHit(float arg1, float arg2, float arg3)
    {
        currentCount = CurrentCount + Mathf.Abs((int)(arg3));
        if (OnQuestProgressChange != null)
            OnQuestProgressChange(CurrentCount, TargetCount);
        if (CurrentCount > TargetCount)
        {
            ReadyToReward();
        }
    }
    
    public override void Clear()
    {
        MainController.Instance.level.MainHero.OnGetHit -= OnGetHit;
    }
    public override string AppearMessage()
    {
        return "Get some damage:" + TargetCount;
    }

    public override string PauseMessage()
    {
        return "Get damage from monsters, and try not to die: " + CurrentCount + "/" + TargetCount + "\n" + DifficultyStr();
    }
}

