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
            currentCount++;
            base.OnEnemyDeadCallback(obj);
        }
    }

    public override string AppearMessage()
    {
        return "Destroy overcharged monsters:" + NeedToComplete;
    }

    public override string PauseMessage()
    {
        return "Destroy overcharged monsters: " + currentCount + "/" + NeedToComplete + "\n" + DifficultyStr();
    }
}

