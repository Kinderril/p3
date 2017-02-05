using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class QuestCollectResource : QuestLogicBase
{
    private CraftItemType _craftItemType;
    public QuestCollectResource(QuestGiver QuestGiver, int targetCount, CraftItemType craftItemType,Action<int, int> OnQuestProgressChange) : base(QuestGiver, targetCount, OnQuestProgressChange)
    {
        this._craftItemType = craftItemType;
        MainController.Instance.level.OnCraftItemCollected += OnCraftItemCollected;
    }

    private void OnCraftItemCollected(CraftItemType arg1, int arg2)
    {
        if (_craftItemType == arg1)
        {
            currentCount = CurrentCount + 1;
            if (OnQuestProgressChange != null)
                OnQuestProgressChange(CurrentCount, TargetCount);
            if (CurrentCount >= TargetCount)
            {
                Clear();
                ReadyToReward();
            }
        }
    }


    public override void Clear()
    {
        MainController.Instance.level.OnCraftItemCollected -= OnCraftItemCollected;
    }
    public override string AppearMessage()
    {
        return "Collect resouces:" + TargetCount;
    }

    public override string PauseMessage()
    {
        return "Collect:" + _craftItemType.ToString() + ". Progress:" + CurrentCount + "/" + TargetCount + "\n" + DifficultyStr();
    }
}

