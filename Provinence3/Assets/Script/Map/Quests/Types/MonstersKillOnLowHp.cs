using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MonstersKillOnLowHp : MonsterKillBase
{
    private Hero hero;
    private float targetHp;
    public MonstersKillOnLowHp(QuestGiver QuestGiver, int NeedToComplete, float coef,Action<int, int> OnQuestProgressChange) 
        : base(QuestGiver, NeedToComplete, OnQuestProgressChange)
    {
        hero = MainController.Instance.level.MainHero;
        targetHp = hero.Parameters.MaxHp*coef;
    }

    protected override void OnEnemyDeadCallback(Unit obj)
    {
        if (hero.CurHp < targetHp)
        {
            currentCount++;
            base.OnEnemyDeadCallback(obj);
        }
    }

    public override string AppearMessage()
    {
        return "Destroy monsters on low health:" + NeedToComplete;
    }

    public override string PauseMessage()
    {
        return "Destroy monsters, when your health less than 33%: " + currentCount + "/" + NeedToComplete + "\n" + DifficultyStr();
    }
}

