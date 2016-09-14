using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class MonstersKillWeaponType : MonsterKillBase
{
    private SourceType SourceType;
    private WeaponType WeaponType;
    public MonstersKillWeaponType(QuestGiver QuestGiver, int NeedToComplete, SourceType SourceType, WeaponType WeaponType, Action<int, int> OnQuestProgressChange) 
        : base(QuestGiver, NeedToComplete, OnQuestProgressChange)
    {
        this.SourceType = SourceType;
        this.WeaponType = WeaponType;
    }

    protected override void OnEnemyDeadCallback(Unit obj)
    {
        if (obj.LastHitInfo.SourceType == SourceType && obj.LastHitInfo.DamageType == WeaponType)
        {
            currentCount++;
            base.OnEnemyDeadCallback(obj);
        }
    }
    public override string AppearMessage()
    {
        return "Destroy monsters with crossbow:" + NeedToComplete;
    }

    public override string PauseMessage()
    {
        return "Use crossbow to kill some monsters: " + currentCount + "/" + NeedToComplete + "\n" + DifficultyStr();
    }
}

