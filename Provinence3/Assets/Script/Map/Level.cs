using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
    public Action<BossUnit> OnBossAppear;
    public Action<CraftItemType, int> OnCraftItemCollected;
    public Action OnEndLevel;

    public Energy Energy;
    public Hero MainHero;
    private DictionaryOfItemAndInt moneyInv;
    public int difficult = 1;
    public bool isPLaying ;
    private PortalsController PortalsController = new PortalsController();
    private List<BaseItem> collectedItems = new List<BaseItem>();
    private Dictionary<CraftItemType,int> collectedCrafts = new Dictionary<CraftItemType, int>(); 
    public int MissionIndex = 1;
    public int IndexBornPoint = 0;
    public EndlevelType IsGoodEnd;
    public int EnemiesKills = 0;
    private float penalty;

    public Level(int levelIndex,int indexBornPos,int difficult,Action<Level> callback)
    {

        TimeUtils.StartMeasure("LOAD PRELEVEL");
        Energy = new Energy(ActivaAction,OnRage);
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
        TimeUtils.EndMeasure("LOAD PRELEVEL");
        MainHero = Map.Instance.Init(this, levelIndex ,indexBornPos);
//        PortalsController.Start((int)maxpower,OnPortalOpen);
        isPLaying = false;
        callback(this);

    }

    private void OnRage()
    {
        MainHero.Rage();
    }

    public void Start()
    {
        isPLaying = true;
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

    private void OnPortalOpen()
    {
        //TODO
//        Vector3 placeToBorn;
        //Find closes bornPositions
    }

    public void MessageAppear(string txt,Color color , Sprite icon)
    {

        var item = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberWithPicture);
        item.transform.SetParent(WindowManager.Instance.CurrentWindow.transform);
        item.Init(txt, color, icon);
    }
    public void AddItem(ItemId type, int value)
    {
        switch (type)
        {
            case ItemId.money:
            case ItemId.crystal:
                value = (int)(value * (MainHero.moneyBonusFromItem + 1f));
                moneyInv[type] += value;
                ActivaAction(type, value);
                break;
            case ItemId.energy:
                Energy.Add(value);
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
        if (isPLaying)
        {
            Energy.Update();
        }
    }

    public void EndLevel(PlayerData PlayerData, EndlevelType LevelEndType)
    {
        IsGoodEnd = LevelEndType;
        PortalsController.Stop();
        MainHero.Control.enabled = false;
        var isBad = LevelEndType == EndlevelType.bad;
#if UNITY_EDITOR
        if (DebugController.Instance.ALWAYS_GOOD_END)
        {
            isBad = false;
        }
#endif
        if (isBad)
        {
            moneyInv.Remove(ItemId.crystal);
            moneyInv[ItemId.money] /= 2;
        }
        else
        {
            AddRandomGift();
            foreach (var collectedItem in collectedItems)
            {
                PlayerData.AddItem(collectedItem, false);
            }
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
        Energy.Dispose();
        MainController.Instance.StartCoroutine(WaitEndLvl());
    }

    private IEnumerator WaitEndLvl()
    {
        yield return new WaitForFixedUpdate();
        DataBaseController.Instance.Pool.Clear();
    } 

    private void AddRandomGift()
    {
        if (Energy.MorePowerLeft())
        {
            IsGoodEnd = EndlevelType.good;
        }

        WDictionary<GiftType> gifts = new WDictionary<GiftType>(new Dictionary<GiftType, float>()
        {
            { GiftType.catalys, 6 },
            { GiftType.recepi ,5 },
            { GiftType.item ,2 },
            { GiftType.enchant ,8 },
            { GiftType.bonus ,3 },
        });
        var val = gifts.Random();


        int lvl = MainController.Instance.PlayerData.Level;
        switch (val)
        {
            case GiftType.catalys:
                collectedItems.Add(ExecCatalysItem.Creat());
                break;
            case GiftType.recepi:
                collectedItems.Add(HeroShopRecipeItem.CreatRandomRecipeItem(lvl));
                break;
            case GiftType.item:
                var item = HeroShopRandomItem.CreatMainSlot(ShopController.RandomSlot(), lvl);
                collectedItems.Add(item);
                break;
            case GiftType.enchant:
                collectedItems.Add(ExecEnchantItem.Creat());
                break;
            case GiftType.bonus:
                collectedItems.Add(HeroShopBonusItem.CreatBonusItem(lvl));
                break;
        }
    }

    public DictionaryOfItemAndInt GetAllCollectedMoney()
    {
        return moneyInv;
    }

    public void EnemieDead()
    {
        EnemiesKills++;
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

}

