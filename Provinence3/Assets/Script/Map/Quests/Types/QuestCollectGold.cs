using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class QuestCollectGold : QuestLogicBase
{
    private ItemId itemType;
    public QuestCollectGold(QuestGiver QuestGiver, int NeedToComplete, ItemId itemType, Action<int, int> OnQuestProgressChange) : base(QuestGiver, NeedToComplete, OnQuestProgressChange)
    {
        this.itemType = itemType;
        MainController.Instance.level.OnItemCollected += OnItemCollected;
    }

    private void OnItemCollected(ItemId arg1, float arg2, float arg3)
    {
        if (itemType == arg1)
        {
            currentCount+=(int)arg2;
            if (currentCount >= NeedToComplete)
            {
                ReadyToReward();
            }
        }
    }

    public override void Clear()
    {
        MainController.Instance.level.OnItemCollected -= OnItemCollected;
    }
}

