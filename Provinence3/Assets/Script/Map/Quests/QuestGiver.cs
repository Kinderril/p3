
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum QuestStatus
{
    free,
    started,
    blocked,
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
    public GameObject blockStatus;
    public LevelQuestController Controller;
    public int id;
    public QuestLogicType type;
    public QuestStatus QuestStatus = QuestStatus.free;
    public QuestStatus OldStatus = QuestStatus.free;
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
            blockStatus.gameObject.SetActive(QuestStatus != QuestStatus.blocked);
            freeStatus.gameObject.SetActive(QuestStatus == QuestStatus.free);
            readyStatus.gameObject.SetActive(QuestStatus == QuestStatus.ready);
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
        Utils.GroundTransform(transform);
        Status = QuestStatus.free;

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
        if (Status != QuestStatus.ready && Status != QuestStatus.end)
        {
            Status = QuestStatus.ready;
            Controller.Check(this);
        }
//        if (StatusMessage == QuestStatus.started)
//        {
//            StatusMessage = QuestStatus.ready;
//            Controller.Ready(this);
//            logic.Clear();
//        }
    }

    public bool IsReady()
    {
        return Status == QuestStatus.ready;
    }

    public void Reward(Level level,Action<QuestGiver> callback )
    {
        var rewardType = Formuls.RandomQuestReward(difficulty);
        var levelDif = Controller.Level.difficult;
#if UNITY_EDITOR
        if (DebugController.Instance.QUEST_REWARD_ITEM)
        {
            rewardType = QuestRewardType.item;
        }
#endif

        float diffCoef = 1f;
        switch (rewardType)
        {
            case QuestRewardType.money:
                float c = UnityEngine.Random.Range(3f, 5f);
                var gold = Formuls.GoldInChest(levelDif);
                switch (difficulty)
                {
                    case QuestDifficulty.easy:
                        diffCoef = 0.6f;
                        break;
                    case QuestDifficulty.hard:
                        diffCoef = 1.45f;
                        break;
                }
                Controller.Level.AddItem(ItemId.money,(int)(diffCoef*gold*c));
                break;
            case QuestRewardType.materials:
                float r = UnityEngine.Random.Range(4f, 6f);
                switch (difficulty)
                {
                    case QuestDifficulty.easy:
                        diffCoef = 0.6f;
                        break;
                    case QuestDifficulty.hard:
                        diffCoef = 1.45f;
                        break;
                }
                var craftType = DataBaseController.Instance.GetRandomCraftItemType();
                Controller.Level.AddItem(craftType, (int)(r* diffCoef));
                break;
            case QuestRewardType.crystal:
                switch (difficulty)
                {
                    case QuestDifficulty.easy:
                        diffCoef = 0.6f;
                        break;
                    case QuestDifficulty.hard:
                        diffCoef = 1.45f;
                        break;
                }
                float cr = UnityEngine.Random.Range(1f, 3.1f);
                Controller.Level.AddItem(ItemId.crystal, (int)(cr * diffCoef));
                break;
            case QuestRewardType.item:
                GiftType giftType = Formuls.CalcGiftType(difficulty);
                Controller.Level.AddRandomGift(true, giftType);
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
        float coef = 1;
        switch (difficulty)
        {
            case QuestDifficulty.easy:
                coef = 0.6f;
                break;
            case QuestDifficulty.hard:
                coef = 1.6f;
                break;
        }

        int r;
        var typ = Controller.GetRandomQuest();
        switch (typ)
        {
            case QuestLogicType.killName:
                r = (int)(UnityEngine.Random.Range(7.5f, 9.5f)*coef);
                logic = new MonsterKillByName(this, Map.Instance.enemies[0].name, r, OnQuestProgressChange);
                break;
            case QuestLogicType.killLowHp:
                r = (int)(UnityEngine.Random.Range(6.5f, 8.5f) * coef);
                logic = new MonstersKillOnLowHp(this, r, 0.5f, OnQuestProgressChange);
                break;
            case QuestLogicType.killDistance:
                r = (int)(UnityEngine.Random.Range(6f, 8f) * coef);
                logic = new MonsterKillDistance(this, 5,r, OnQuestProgressChange);
                break;
            case QuestLogicType.killCrossbow:
                r = (int)(UnityEngine.Random.Range(9.5f, 11.5f) * coef);
                logic = new MonstersKillWeaponType(this, r, SourceType.weapon,WeaponType.magic, OnQuestProgressChange);
                break;
            case QuestLogicType.killTalisman:
                r = (int)(UnityEngine.Random.Range(6f, 8f) * coef);
                logic = new MonstersKillWeaponType(this, r, SourceType.talisman, WeaponType.magic, OnQuestProgressChange);
                break;
            case QuestLogicType.killOvercharged:
                r = (int)(UnityEngine.Random.Range(4f, 6f) * coef);
                logic = new MonsterKillOvercharged(this, r, OnQuestProgressChange);
                break;
            case QuestLogicType.collectGold:
                var g = Formuls.GoldInChest(MainController.Instance.level.difficult);
                r = (int)(g * UnityEngine.Random.Range(6f, 9f) * coef);
                logic = new QuestCollectGold(this,r,ItemId.money, OnQuestProgressChange);
                break;
            case QuestLogicType.collectReource:
                r = (int)(UnityEngine.Random.Range(5f, 7f) * coef);
                var enemies = Map.Instance.enemies;
                var enemy = enemies.RandomElement();
                if (enemy != null)
                {
                    logic = new QuestCollectResource(this, r, enemy.ParametersScriptable.DropItem.type_rare, OnQuestProgressChange);
                }
                break;
            case QuestLogicType.getDamage:
                var h = MainController.Instance.level.MainHero.Parameters.MaxHp;
                r = (int)(UnityEngine.Random.Range(0.8f, 2.3f) * h * coef);
                logic = new QuestGetDamage(this, r, OnQuestProgressChange);
                break;
            default:
                logic = new QuestGetDamage(this,500, OnQuestProgressChange);
                break;
        }

        Debug.Log("Quest Activated");
//        logic = new MonsterKillOvercharged(this, 5, OnQuestProgressChange);
        if (callback != null && logic != null)
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

    public void SetBlock()
    {
        OldStatus = Status;
        Status = QuestStatus.blocked;

    }

    public void UnBlock()
    {
        Status = OldStatus;
    }
}

