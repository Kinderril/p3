using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MonsterKillByName : MonsterKillBase
{
    private string name ;
    public MonsterKillByName(QuestGiver QuestGiver, string name,int need, Action<int, int> OnQuestProgressChange)
        : base(QuestGiver,need, OnQuestProgressChange)
    {
        this.name = name;
    }

    protected override void OnEnemyDeadCallback(Unit obj)
    {
        var monster = obj as BaseMonster;
        if (monster != null && monster.name.Equals(name))
        {
            currentCount = CurrentCount + 1;
            base.OnEnemyDeadCallback(obj);
        }
    }
    public override string AppearMessage()
    {
        return "Kill some type of monster:" + TargetCount;
    }

    public override string PauseMessage()
    {
        return "Destroy monsters:" + name + " Progress:" + CurrentCount + "/" + TargetCount + "\n" + DifficultyStr();
    }
}

