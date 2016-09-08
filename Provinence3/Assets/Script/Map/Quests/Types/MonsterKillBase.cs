using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class MonsterKillBase : QuestLogicBase
{
    protected Hero hero;
    protected MonsterKillBase(QuestGiver QuestGiver,int NeedToComplete, Action<int, int> OnQuestProgressChange)
        : base(QuestGiver, NeedToComplete, OnQuestProgressChange)
    {
        hero = MainController.Instance.level.MainHero;
        Map.Instance.OnEnemyDeadCallback += OnEnemyDeadCallback;
    }

    protected virtual void OnEnemyDeadCallback(Unit obj)
    {
        if (OnQuestProgressChange != null)
        {
            OnQuestProgressChange(currentCount, NeedToComplete);
        }
        if (currentCount >= NeedToComplete)
        {
            ReadyToReward();
        }
    }


    public override void Clear()
    {
        Map.Instance.OnEnemyDeadCallback -= OnEnemyDeadCallback;
    }
}
