using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LevelQuestController
{
    private QuestGiver currentActiveQuest;
    private int totalQuests;
    private List<int> finishedQuests = new List<int>(); 
    private List<QuestGiver> Quests = new List<QuestGiver>();
    public Level Level;
    public Action<QuestGiver> OnQuestStatusChanges; 
    public Action<QuestGiver,int ,int> OnQuestProgress;
    private List<QuestLogicType> AllQuestsTypes = new List<QuestLogicType>(); 

    public LevelQuestController(Level level)
    {
        this.Level = level;
        foreach (QuestLogicType a in Enum.GetValues(typeof(QuestLogicType)))
        {
            AllQuestsTypes.Add(a);
        }
    }

    public QuestLogicType GetRandomQuest()
    {
        return AllQuestsTypes.RandomElement();
    }

    public void Statistics(out int cur, out int total)
    {
        cur = finishedQuests.Count;
        total = totalQuests;
    }

    public string CurrentQuestInfo()
    {
        return currentActiveQuest == null ? "I don't have any quests" : currentActiveQuest.Info();
    }

    public void Start(Level level)
    {
        totalQuests = Quests.Count;
    }

    public void Clear()
    {
        Level = null;
        foreach (var questGiver in Quests)
        {
            questGiver.OnDestroyGiver -= OnDestroyGiver;
        }

        foreach (var questGiver in Quests)
        {
            GameObject.Destroy(questGiver.gameObject);
        }
    }

    public void Check(QuestGiver questGiver)
    {
        if (currentActiveQuest == null)
        {
            if (!finishedQuests.Contains(questGiver.id))
            {
                Start(questGiver);
#if UNITY_EDITOR
                if (DebugController.Instance.QUEST_COMPLETE)
                {
                    End(questGiver);
                }
#endif
            }
        }
        else
        {
            if (currentActiveQuest == questGiver)
            {
                if (questGiver.IsReady())
                {
                    End(questGiver);
                }
            }
        }
    }

    private void End(QuestGiver questGiver)
    {
        currentActiveQuest = null;
        questGiver.Reward(Level, giver =>
        {
            OnQuestStatusChanges(giver);
            foreach (var quest in Quests.Where(x=>x != giver))
            {
                quest.UnBlock();
            }
        });
    }

    private void Start(QuestGiver questGiver)
    {
        currentActiveQuest = questGiver;
        questGiver.SetCallBack(OnProgress);
        questGiver.Activate(giver =>
        {
            OnQuestStatusChanges(giver);
            foreach (var quest in Quests.Where(x=>x != giver))
            {
                quest.SetBlock();
            }
        });
    }

    private void OnProgress(int cur, int target)
    {
        if (OnQuestProgress != null)
        {
            OnQuestProgress(currentActiveQuest,cur, target);
        }
    }
    public void Ready(QuestGiver questGiver)
    {
        if (OnQuestStatusChanges != null)
        {
            OnQuestStatusChanges(questGiver);
        }
    }

    public void Add(QuestGiver giver)
    {
        Quests.Add(giver);
        giver.OnDestroyGiver += OnDestroyGiver;
    }

    private void OnDestroyGiver(QuestGiver obj)
    {
        Quests.Remove(obj);
    }
}

