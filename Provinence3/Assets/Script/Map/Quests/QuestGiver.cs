
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum QuestStatus
{
    free,
    started,
    ready,
    end,
}

public enum QuestType
{
    f,e,//TODO
}

public enum QuestDifficulty
{
    easy,
    normal,
    hard,
}
public enum QuestRewardType
{
    money,
    materials,
    crystal,
    item,
}

public class QuestGiver : MonoBehaviour
{
    public GameObject freeStatus;
    public GameObject readyStatus;
    public LevelQuestController Controller;
    public int id;
    public QuestType type;
    public QuestStatus QuestStatus = QuestStatus.free;
    public event Action<QuestGiver> OnDestroyGiver;
    public event Action<int, int> OnQuestProgressChange; 
    private QuestDifficulty Difficulty;
    public BaseEffectAbsorber GetRewardEffect;
    private QuestLogicBase Logic;

    public QuestStatus Status
    {
        get { return QuestStatus; }
        set
        {
            QuestStatus = value;
            switch (QuestStatus)
            {
                case QuestStatus.free:
                    freeStatus.gameObject.SetActive(true);
                    readyStatus.gameObject.SetActive(false);
                    break;
                case QuestStatus.ready:
                    readyStatus.gameObject.SetActive(true);
                    freeStatus.gameObject.SetActive(false);
                    break;
                default:
                    freeStatus.gameObject.SetActive(false);
                    readyStatus.gameObject.SetActive(false);
                    break;
            }
        }
    }

    public void Init(LevelQuestController controller)
    {
        freeStatus.gameObject.SetActive(false);
        readyStatus.gameObject.SetActive(false);
        this.Controller = controller;
        Difficulty = Formuls.RandomQuestDifficulty();

    }

    void OnTriggerEnter(Collider other)
    {
        if (Status != QuestStatus.end)
        {
            var monster = other.GetComponent<Hero>();
            if (monster != null)
            {
                Controller.Check(this);
            }
        }
    }

    public void SetReady()
    {
        if (Status == QuestStatus.started)
        {
            Status = QuestStatus.ready;
            Controller.Ready(this);
            Logic.Clear();
        }
    }

    public bool IsReady()
    {
        return Status == QuestStatus.ready;
    }

    public void Reward(Level level,Action<QuestGiver> callback )
    {
        var rewardType = Formuls.RandomQuestReward(Difficulty);
        var levelDif = Controller.Level.difficult;

        switch (rewardType)
        {
            case QuestRewardType.money:
                Controller.Level.AddItem(ItemId.money, 130);
                break;
            case QuestRewardType.materials:
                Controller.Level.AddItem(CraftItemType.Bone, 30);
                break;
            case QuestRewardType.crystal:
                Controller.Level.AddItem(ItemId.crystal, 1);
                break;
            case QuestRewardType.item:
                Controller.Level.AddRandomGift(true);
                break;
        }
        Status = QuestStatus.end;
        if (GetRewardEffect != null)
        {
            GetRewardEffect.Play();
        }
        if (callback != null)
        {
            callback(this);
        }
        StartCoroutine(WaitLoPlayEffect());
    }


    private IEnumerator WaitLoPlayEffect()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    } 
    public void Activate(Action<QuestGiver> callback)
    {
        Status = QuestStatus.started;
        if (callback != null)
        {
            callback(this);
        }
        Logic = new MonsterKillByName(this,"dog",5,OnQuestProgressChange);
    }

    public string Info()
    {
        return "adsfds";//TODO

    }

    void OnDestroy()
    {
        if (Logic != null)
        {
            Logic.Clear();
        }
        if (OnDestroyGiver != null)
        {
            OnDestroyGiver(this);
        }
    }

    public void SetCallBack(Action<int, int> onProgress)
    {
        OnQuestProgressChange = onProgress;
    }
}

