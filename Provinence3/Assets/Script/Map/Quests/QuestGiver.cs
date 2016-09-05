
using System;
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

public class QuestGiver : MonoBehaviour
{
    public GameObject freeStatus;
    public GameObject readyStatus;
    private LevelQuestController controller;
    public int id;
    public QuestType type;
    public QuestStatus QuestStatus = QuestStatus.free;
    public event Action<QuestGiver> OnDestroyGiver;

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
        level.AddItem(ItemId.money,400);//TODO
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

