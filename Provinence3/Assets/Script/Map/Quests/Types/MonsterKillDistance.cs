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
            currentCount++;
            base.OnEnemyDeadCallback(obj);
        }
    }
}

