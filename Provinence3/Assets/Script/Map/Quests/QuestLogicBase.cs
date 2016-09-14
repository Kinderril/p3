using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum QuestLogicType
{
    killName,
    killLowHp,
    killDistance,

    killCrossbow,
    killTalisman,
    collectGold,

    collectReource,
    killOvercharged,
    getDamage,
}

public abstract class QuestLogicBase
{
    public QuestGiver QuestGiver;
    protected int targetCount;
    protected int currentCount;
    protected Action<int, int> OnQuestProgressChange;
    public QuestLogicBase(QuestGiver QuestGiver, int targetCount, Action<int, int> OnQuestProgressChange)
    {
        this.OnQuestProgressChange = OnQuestProgressChange;
        this.targetCount = targetCount;
        this.QuestGiver = QuestGiver;
    }

    public int CurrentCount
    {
        get { return currentCount; }
    }

    public int TargetCount
    {
        get { return targetCount; }
    }

    public virtual void Clear()
    {

    }
    protected void ReadyToReward()
    {
        QuestGiver.SetReady();
    }

    public virtual string AppearMessage()
    {
        return "";
    } 
    public virtual string PauseMessage()
    {
        return "";
    }

    protected string DifficultyStr()
    {
        return "\n Diffictulty" + QuestGiver.Difficulty.ToString();
    }
}
