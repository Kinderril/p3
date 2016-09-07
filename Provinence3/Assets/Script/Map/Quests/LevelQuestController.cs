﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


public class LevelQuestController
{
    private QuestGiver currentActiveQuest;
    private int totalQuests;
    private List<int> finishedQuests = new List<int>(); 
    private List<QuestGiver> Quests = new List<QuestGiver>();
    public Level Level;
    public Action<QuestGiver> OnQuestStatusChanges; 

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
        questGiver.Reward(Level,OnQuestStatusChanges);
    }

    private void Start(QuestGiver questGiver)
    {
        currentActiveQuest = questGiver;
        questGiver.Activate(OnQuestStatusChanges);
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

