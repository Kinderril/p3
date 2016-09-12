
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
    public QuestLogicType type;
    public QuestStatus QuestStatus = QuestStatus.free;
    public event Action<QuestGiver> OnDestroyGiver;
    public event Action<int, int> OnQuestProgressChange; 
    private QuestDifficulty difficulty;
    public BaseEffectAbsorber GetRewardEffect;
    private QuestLogicBase logic;

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

    public QuestLogicBase Logic
    {
        get { return logic; }
    }

    public QuestDifficulty Difficulty
    {
        get { return difficulty; }
    }

    public void Init(LevelQuestController controller)
    {
        freeStatus.gameObject.SetActive(false);
        readyStatus.gameObject.SetActive(false);
        this.Controller = controller;
        difficulty = Formuls.RandomQuestDifficulty();

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
            logic.Clear();
        }
    }

    public bool IsReady()
    {
        return Status == QuestStatus.ready;
    }

    public void Reward(Level level,Action<QuestGiver> callback )
    {
        var rewardType = Formuls.RandomQuestReward(difficulty);
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
//        var typ = Controller.GetRandomQuest();
//        switch (typ)
//        {
//            case QuestLogicType.killName:
//                logic = new MonsterKillByName(this, "dog", 5, OnQuestProgressChange);
//                break;
//            case QuestLogicType.killLowHp:
//                logic = new MonstersKillOnLowHp(this, 5, 0.3f, OnQuestProgressChange);
//                break;
//            case QuestLogicType.killDistance:
//                logic = new MonsterKillDistance(this, 5,5, OnQuestProgressChange);
//                break;
//            case QuestLogicType.killCrossbow:
//                logic = new MonstersKillWeaponType(this, 5, SourceType.weapon,WeaponType.magic, OnQuestProgressChange);
//                break;
//            case QuestLogicType.killTalisman:
//                logic = new MonstersKillWeaponType(this, 5, SourceType.talisman, WeaponType.magic, OnQuestProgressChange);
//                break;
//            case QuestLogicType.killOvercharged:
//                logic = new MonsterKillOvercharged(this, 5, OnQuestProgressChange);
//                break;
//            case QuestLogicType.collectGold:
//                logic = new QuestCollectGold(this,400,ItemId.money, OnQuestProgressChange);
//                break;
//            case QuestLogicType.collectReource:
//                logic = new QuestCollectResource(this, 10, CraftItemType.Leather, OnQuestProgressChange);
//                break;
//            case QuestLogicType.getDamage:
//                logic = new QuestGetDamage(this, 500, OnQuestProgressChange);
//                break;
//            default:
//                logic = new QuestGetDamage(this,500, OnQuestProgressChange);
//                break;
//        }

        Debug.Log("Quest Activated");
        logic = new MonsterKillOvercharged(this, 5, OnQuestProgressChange);
        if (callback != null)
        {
            callback(this);
        }
    }

    public string Info()
    {
        return Logic.PauseMessage();
    }

    void OnDestroy()
    {
        if (logic != null)
        {
            logic.Clear();
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

