using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MonsterKillOvercharged : MonsterKillBase
{
    private string name ;
    public MonsterKillOvercharged(QuestGiver QuestGiver, int need, Action<int, int> OnQuestProgressChange)
        : base(QuestGiver,need, OnQuestProgressChange)
    {

    }

    protected override void OnEnemyDeadCallback(Unit obj)
    {
        var monster = obj as BaseMonster;
        if (monster != null && monster.Overcharged)
        {
            currentCount = CurrentCount + 1;
            base.OnEnemyDeadCallback(obj);
        }
    }

    public override string AppearMessage()
    {
        return "Destroy overcharged monsters:" + TargetCount;
    }

    public override string PauseMessage()
    {
        return "Destroy overcharged monsters: " + CurrentCount + "/" + TargetCount + "\n" + DifficultyStr();
    }
}

