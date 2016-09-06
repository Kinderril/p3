
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
    private LevelQuestController controller;
    public int id;
    public QuestType type;
    public QuestStatus QuestStatus = QuestStatus.free;
    public event Action<QuestGiver> OnDestroyGiver;
    private QuestDifficulty Difficulty;
    public BaseEffectAbsorber GetRewardEffect;

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
        this.controller = controller;
        Difficulty = Formuls.RandomQuestDifficulty();

    }

    void OnTriggerEnter(Collider other)
    {
        if (Status != QuestStatus.end)
        {
            var monster = other.GetComponent<Hero>();
            if (monster != null)
            {
                controller.Check(this);
            }
        }
    }

    public bool Ready()
    {
        if (Status == QuestStatus.started)
        {
        }
        return false;
    }

    public void Reward(Level level)
    {
        Status = QuestStatus.end;
        var rewardType = Formuls.RandomQuestReward(Difficulty);
        var levelDif = controller.Level.difficult;
        switch (rewardType)
        {
            case QuestRewardType.money:
                controller.Level.AddItem(ItemId.money, 130);
                break;
            case QuestRewardType.materials:
                controller.Level.AddItem(CraftItemType.Bone, 30);
                break;
            case QuestRewardType.crystal:
                controller.Level.AddItem(ItemId.crystal, 1);
                break;
            case QuestRewardType.item:
                var item = new PlayerItem(new Dictionary<ParamType, float>(), Slot.Talisman, Rarity.Magic, 1);
                controller.Level.AddItem(item);
                break;
        }
        if (GetRewardEffect != null)
        {
            GetRewardEffect.Play();
        }
        StartCoroutine(WaitLoPlayEffect());
    }


    private IEnumerator WaitLoPlayEffect()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    } 
    public void Activate()
    {
        Status = QuestStatus.started;
    }

    public string Info()
    {
        return "adsfds";//TODO

    }

    void OnDestroy()
    {
        if (OnDestroyGiver != null)
        {
            OnDestroyGiver(this);
        }
    }
}

