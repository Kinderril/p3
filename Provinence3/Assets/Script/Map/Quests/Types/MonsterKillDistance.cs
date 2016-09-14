using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MonsterKillDistance : MonsterKillBase
{
    private float dist;
    public MonsterKillDistance(QuestGiver QuestGiver, float dist,int need, Action<int, int> OnQuestProgressChange)
        : base(QuestGiver,need, OnQuestProgressChange)
    {
        this.dist = dist* dist;
    }

    protected override void OnEnemyDeadCallback(Unit obj)
    {
        var monster = obj as BaseMonster;
        if (monster.mainHeroDist > dist)
        {
            currentCount = CurrentCount + 1;
            base.OnEnemyDeadCallback(obj);
        }
    }
    public override string AppearMessage()
    {
        return "Destroy monsters from distance:" + TargetCount;
    }

    public override string PauseMessage()
    {
        return "You must kill monsters from big distance. Become sniper: " + CurrentCount + "/" + TargetCount + "\n" + DifficultyStr();
    }
}

