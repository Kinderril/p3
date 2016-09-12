using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class QuestCollectResource : QuestLogicBase
{
    private CraftItemType _craftItemType;
    public QuestCollectResource(QuestGiver QuestGiver, int NeedToComplete, CraftItemType craftItemType,Action<int, int> OnQuestProgressChange) : base(QuestGiver, NeedToComplete, OnQuestProgressChange)
    {
        this._craftItemType = craftItemType;
        MainController.Instance.level.OnCraftItemCollected += OnCraftItemCollected;
    }

    private void OnCraftItemCollected(CraftItemType arg1, int arg2)
    {
        if (_craftItemType == arg1)
        {
            currentCount++;
            if (currentCount >= NeedToComplete)
            {
                ReadyToReward();
            }
        }
    }


    public override void Clear()
    {
        MainController.Instance.level.OnCraftItemCollected -= OnCraftItemCollected;
    }
}

