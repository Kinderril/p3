using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GiftType
{
    catalys,
    recepi,
    item,
    enchant,
    bonus,
}

public enum EndlevelType
{
    bad,
    normal,
    good
}

[Serializable]
public struct inventotyItem
{
    public ItemId id;
    public int count;

    public inventotyItem(ItemId id)
    {
        this.id = id;
        count = 0;
    }
}
[Serializable]
public class DictionaryOfItemAndInt : SerializableDictionary<ItemId, int> { }

public class Level
{
    public Action<ItemId,float,float> OnItemCollected;
    public Action<BaseItem> OnPlayerItemCollected;
    public Action<BossUnit> OnBossAppear;
    public Action<CraftItemType, int> OnCraftItemCollected;
    public Action OnEndLevel;
    public Action<EndlevelType> OnPreEndLevel;
    public Action<bool> OnPause;

    public Energy Energy;
    public Ammo Ammo;
    public Hero MainHero;
    private DictionaryOfItemAndInt moneyInv;
    public int difficult = 1;
    public bool IsPlaying ;
    private PortalsController PortalsController = new PortalsController();
    private List<BaseItem> collectedItems = new List<BaseItem>();
    private Dictionary<CraftItemType,int> collectedCrafts = new Dictionary<CraftItemType, int>(); 
    public int MissionIndex = 1;
    public int IndexBornPoint = 0;
    public EndlevelType IsGoodEnd;
    private float penalty;
    public float MoneyBonusFromItem = 1f;
    public LevelQuestController QuestController;
    public LevelStatistics LevelStatistics;
    public LevelObject levelObject = null;
    private EndlevelType LevelEndType;

    public Level(int levelIndex,int indexBornPos,int difficult)
    {
        LevelStatistics = new LevelStatistics();
        TimeUtils.StartMeasure("MAIN");
        TimeUtils.StartMeasure("LOAD PRELEVEL");
        CrystalsBonus = 1;
        QuestController = new LevelQuestController(this);
        Energy = new Energy(ActivaAction,OnRage);
        Ammo = new Ammo(ActivaAction);
        MissionIndex = levelIndex;
        IndexBornPoint = indexBornPos;
        this.difficult = difficult;
        DataBaseController.Instance.Pool.StartLevel();
        penalty = GetPenalty(difficult);
        moneyInv = new DictionaryOfItemAndInt();
        foreach (ItemId id in Enum.GetValues(typeof(ItemId)))
        {
            moneyInv.Add(id,0);
        }
        DebugController.Instance.InfoField2.text += TimeUtils.EndMeasure("LOAD PRELEVEL");


    

    }

    public IEnumerator Load(Action<Level> callback)
    {
        var sceneName = "Level" + MissionIndex;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        var scene = SceneManager.GetSceneByName(sceneName);
        while (!scene.isLoaded)
        {
            yield return new WaitForSeconds(1f);
        }
        
        foreach (var rootGameObject in scene.GetRootGameObjects())
        {
            levelObject = rootGameObject.GetComponent<LevelObject>();
            if (levelObject != null)
            {
                break;
            }
        }
        //        LoadLevelGameObject(levelIndex);
        if (levelObject != null)
        {
            Utils.Init(levelObject.Terrain);
            MainHero = Map.Instance.Init(this, levelObject, IndexBornPoint);
            DebugController.Instance.InfoField2.text += " " + TimeUtils.EndMeasure("MAIN");
            IsPlaying = false;
            callback(this);
            Map.Instance.StartLoadingMonsters();
        }
        else
        {
            Debug.LogError("WRONG LEVEL LOAD");
        }
        yield return null;
    }

    private void OnRage()
    {
        MainHero.Rage();
    }

    public void StartLevel()
    {
        IsPlaying = true;
        QuestController.Start(this);
        Ammo.AddAmmo(10);
        //        Utils.GroundTransform(MainHero.transform);
    }

    public float Penalty
    {
        get { return penalty; }
    }

    public Dictionary<CraftItemType, int> CollectedCrafts
    {
        get { return collectedCrafts; }
    }

    public List<BaseItem> CollectedItems
    {
        get { return collectedItems; }
    }

    public float CrystalsBonus
    {
        get; set;
    }

    public bool IsPause { get; set; }
    
    public void BigMessageAppear(string txt, string subTxt,Color color,float speed = 1)
    {
        var item = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberWithPicture);
        item.transform.SetParent(WindowManager.Instance.CurrentWindow.transform);
        if (subTxt.Length > 0)
        {
            item.Init(txt, subTxt, color);
        }
        else
        {
            item.Init(txt, color);
        }
    }

    public void Pause()
    {
//        WindowManager.Instance.OpenWindow(MainState.pause);
        //Open window
        Time.timeScale = 0;
        IsPause = true;
        if (OnPause != null)
        {
            OnPause(IsPause);
        }
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        IsPause = false;
        if (OnPause != null)
        {
            OnPause(IsPause);
        }
    }

    public void AddItem(ItemId type, int value)
    {
        switch (type)
        {
            case ItemId.money:
                value = (int)(value * MoneyBonusFromItem);
                moneyInv[type] += value;
                ActivaAction(type, value);
                break;
            case ItemId.crystal:
                moneyInv[type] += value;
                ActivaAction(type, value);
                break;
            case ItemId.energy:
                Energy.Add(value);
                break;
            case ItemId.ammo:
                Ammo.AddAmmo(value);
                break;
            case ItemId.health:
                break;
        }
        
    }

    private void ActivaAction(ItemId i,int delta)
    {
        if (OnItemCollected != null)
        {
            OnItemCollected(i, moneyInv[i], delta);
        }
        
    }


    public void AddItem(BaseItem item)
    {
        collectedItems.Add(item);
    }
    public void AddItem(CraftItemType type, int count)
    {
        if (collectedCrafts.ContainsKey(type))
        {
            collectedCrafts[type] += count;
        }
        else
        {
            collectedCrafts.Add(type,count);
        }
        if (OnCraftItemCollected != null)
        {
            OnCraftItemCollected(type, count);
        }
    }

    public void Update()
    {
        if (IsPlaying)
        {
            Energy.Update();
        }
    }
    public void PreEndLevel(EndlevelType type)
    {
        LevelEndType = type;
        var isTutor = MissionIndex == 0;
        if (isTutor)
        {
            MainController.Instance.PlayerData.TutorEnd();
        }
        IsPlaying = false;
        LevelStatistics.End();
        IsGoodEnd = LevelEndType;
        PortalsController.Stop();
        QuestController.Clear();
        MainHero.Control.enabled = false;

        if (OnPreEndLevel != null)
        {
            OnPreEndLevel(type);
        }
        Energy.Dispose();

    }

    public void EndLevel(PlayerData PlayerData, bool endImmidiatly = false)
    {
        Debug.Log("EndLevel>> goodEnd:" + LevelEndType);

        var isBad = LevelEndType == EndlevelType.bad;
#if UNITY_EDITOR
        if (DebugController.Instance.ALWAYS_GOOD_END)
        {
            isBad = false;
        }
#endif
        if (!isBad)
        {
            IsGoodEnd = EndlevelType.good;
            AddRandomGift();
        }
        if (LevelStatistics.GetReward(QuestController.CompletedQuests(), collectedCrafts,!isBad))
        {
            AddRandomGift();
        }
        foreach (var collectedItem in collectedItems)
        {
            PlayerData.AddItem(collectedItem, false);
        }
        foreach (var collectedCraft in collectedCrafts)
        {
            var exec = new ExecCraftItem(collectedCraft.Key, collectedCraft.Value);
            PlayerData.AddItem(exec);
        }
        PlayerData.AddInventory(moneyInv);
        PlayerData.Save();
        if (OnEndLevel != null)
        {
            OnEndLevel();
        }
        if (endImmidiatly)
        {
            AfterWait();
        }
        else
        {
            MainController.Instance.StartCoroutine(WaitEndLvl());
        }
        PlayerData.AddLevelEnd(this,LevelStatistics);
    }

    private IEnumerator WaitEndLvl()
    {
        yield return new WaitForFixedUpdate();
        AfterWait();
    }

    private void AfterWait()
    {
        DataBaseController.Instance.Pool.Clear();

    }
    
    public void AddRandomGift(bool withAction = false,GiftType? giftType = null)
    {
        if (giftType == null)
        {
            WDictionary<GiftType> gifts = new WDictionary<GiftType>(new Dictionary<GiftType, float>()
            {
                {GiftType.catalys, 6},
                {GiftType.recepi, 5},
                {GiftType.item, 2},
                {GiftType.enchant, 8},
                {GiftType.bonus, 3},
            });
            giftType = gifts.Random();
        }

        int lvl = MainController.Instance.PlayerData.Level;
        BaseItem baseItem = null;
        switch (giftType)
        {
            case GiftType.catalys:
                baseItem = ExecCatalysItem.Creat();
                break;
            case GiftType.recepi:
                baseItem = (HeroShopRecipeItem.CreatRandomRecipeItem(lvl));
                break;
            case GiftType.item:
                var item = HeroShopRandomItem.CreatMainSlot(ShopController.RandomSlot(), lvl);
                baseItem = (item);
                break;
            case GiftType.enchant:
                baseItem = (ExecEnchantItem.Creat());
                break;
            case GiftType.bonus:
                baseItem = (HeroShopBonusItem.CreatBonusItem(lvl));
                break;
        }
        if (baseItem != null)
        {
            collectedItems.Add(baseItem);
            if (withAction)
            {
                if (OnPlayerItemCollected != null)
                {
                    OnPlayerItemCollected(baseItem);
                }
            }
        }
    }

    public DictionaryOfItemAndInt GetAllCollectedMoney()
    {
        return moneyInv;
    }

    public void EnemieDead()
    {
        LevelStatistics.EnemiesKills++;
    }

    public static float GetPenalty(int dif)
    {
        var m = MainController.Instance.PlayerData.Level - dif;

        switch (m)
        {
            case 0:
                return 1f;
            case 1:
                return 0.65f;
            case 2:
                return 0.45f;
            case 3:
                return 0.2f;
            default:
                return 0;
        }
    }

    public void AddQuestGiver(QuestGiver giver)
    {
        giver.Init(QuestController);
        QuestController.Add(giver);
    }

}

