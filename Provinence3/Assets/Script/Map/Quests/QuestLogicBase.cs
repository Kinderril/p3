using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum QuestLogicType
{
    killName,
    killLowHp,
    killMaxHp,

    killCrossbow,
    killTalisman,
    killOvercharged,

    collectGold,
    collectResource,
    getDamage,

}

public abstract class QuestLogicBase
{
    public QuestGiver QuestGiver;
    protected int NeedToComplete;
    protected int currentCount;
    protected Action<int, int> OnQuestProgressChange;
    public QuestLogicBase(QuestGiver QuestGiver, int NeedToComplete, Action<int, int> OnQuestProgressChange)
    {
        this.OnQuestProgressChange = OnQuestProgressChange;
        this.NeedToComplete = NeedToComplete;
        this.QuestGiver = QuestGiver;
    }
    public virtual void Clear()
    {

    }
    protected void ReadyToReward()
    {
        QuestGiver.SetReady();
    }

    public virtual string SubInfo()
    {
        return "";
    }
}
