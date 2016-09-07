using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class QuestLogicBase
{
    public QuestGiver QuestGiver;
    protected int NeedToComplete;
    protected int currentCount;
    public QuestLogicBase(QuestGiver QuestGiver, int NeedToComplete)
    {
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
}
