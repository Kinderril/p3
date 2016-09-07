using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class MonsterKillBase : QuestLogicBase
{
    protected Hero hero;
    protected MonsterKillBase(QuestGiver QuestGiver,int NeedToComplete)
        : base(QuestGiver, NeedToComplete)
    {
        hero = MainController.Instance.level.MainHero;
        Map.Instance.OnEnemyDeadCallback += OnEnemyDeadCallback;
    }

    protected virtual void OnEnemyDeadCallback(Unit obj)
    {
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
