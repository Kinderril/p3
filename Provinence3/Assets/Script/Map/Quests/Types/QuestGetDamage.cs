﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class QuestGetDamage : QuestLogicBase
{
    public QuestGetDamage(QuestGiver QuestGiver, int NeedToComplete,  Action<int, int> OnQuestProgressChange) : base(QuestGiver, NeedToComplete, OnQuestProgressChange)
    {
        MainController.Instance.level.MainHero.OnGetHit += OnGetHit;
    }

    private void OnGetHit(float arg1, float arg2, float arg3)
    {
        currentCount += (int)(arg3);
        if (OnQuestProgressChange != null)
            OnQuestProgressChange(currentCount, NeedToComplete);
        if (currentCount > NeedToComplete)
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
        return "Get some damage:" + NeedToComplete;
    }

    public override string PauseMessage()
    {
        return "Get damage from monsters, and try not to die: " + currentCount + "/" + NeedToComplete + "\n" + DifficultyStr();
    }
}

