using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MonstersKillOnLowHp : MonsterKillBase
{
    private Hero hero;
    private float targetHp;
    public MonstersKillOnLowHp(QuestGiver QuestGiver, int targetCount, float coef,Action<int, int> OnQuestProgressChange) 
        : base(QuestGiver, targetCount, OnQuestProgressChange)
    {
        hero = MainController.Instance.level.MainHero;
        targetHp = hero.Parameters.MaxHp*coef;
    }

    protected override void OnEnemyDeadCallback(Unit obj)
    {
        if (hero.CurHp < targetHp)
        {
            currentCount = CurrentCount + 1;
            base.OnEnemyDeadCallback(obj);
        }
    }

    public override string AppearMessage()
    {
        return "Destroy monsters on low health:" + TargetCount;
    }

    public override string PauseMessage()
    {
        return "Destroy monsters, when your health less than 33%: " + CurrentCount + "/" + TargetCount + "\n" + DifficultyStr();
    }
}

