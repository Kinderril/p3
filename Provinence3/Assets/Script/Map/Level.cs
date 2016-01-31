using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
    private float powerLeft;
    public Hero MainHero;
    private float maxpower = 120;
    public Action<float, float> OnLeft;
    public Action<ItemId,float,float> OnItemCollected;
    public Action<BossUnit> OnBossAppear;
    private DictionaryOfItemAndInt inventory;
    public int difficult = 1;
    public bool isPLaying = true;
    private PortalsController PortalsController = new PortalsController();
    private List<BaseItem> collectedItems = new List<BaseItem>();
    public Action OnEndLevel;
    public int MissionIndex = 1;
    public bool IsGoodEnd;
    public int EnemiesKills = 0;
    private const float speedEnergyFall = 1.5f;

    public Level(int index)
    {
        inventory = new DictionaryOfItemAndInt();
        foreach (ItemId id in Enum.GetValues(typeof(ItemId)))
        {
            inventory.Add(id,0);
        }
        MainHero = Map.Instance.Init(this, index);
        PortalsController.Start((int)maxpower,OnPortalOpen);
    }

    private void OnPortalOpen()
    {
        Vector3 placeToBorn;
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
                inventory[type] += value;
                ActivaAction(type, value);
                break;
            case ItemId.energy:
                powerLeft = Mathf.Clamp(powerLeft + value, -1, maxpower);
                ActionPOwerLeft();
                ActivaAction(type, value);
                break;
            case ItemId.health:
                break;
        }
        
    }

    private void ActivaAction(ItemId i,int delta)
    {
        if (OnItemCollected != null)
        {
            OnItemCollected(i, inventory[i], delta);
        }
        
    }

    private void ActionPOwerLeft()
    {
        if (OnLeft != null)
        {
            OnLeft(powerLeft, maxpower);
        }

        if (powerLeft > maxpower)
        {
            isPLaying = false;
            MainController.Instance.EndLevel(false);
        }
    }

    public void AddItem(BaseItem item)
    {
        collectedItems.Add(item);
    }

    public void Update()
    {
        if (isPLaying)
        {
            powerLeft += Time.deltaTime*speedEnergyFall;
            ActionPOwerLeft();
        }
    }

    public void EndLevel(PlayerData PlayerData,bool isGood)
    {
        IsGoodEnd = isGood;
        PortalsController.Stop();
        MainHero.Control.enabled = false;
        if (!isGood)
        {
            inventory.Remove(ItemId.crystal);
            inventory[ItemId.money] /= 2;
        }
        else
        {
            AddRandomGift();
            foreach (var collectedItem in collectedItems)
            {
                PlayerData.AddItem(collectedItem, false);
            }
        }
        PlayerData.AddInventory(inventory);
        PlayerData.Save();
        if (OnEndLevel != null)
        {
            OnEndLevel();
        }
    }

    private void AddRandomGift()
    {
        if (UnityEngine.Random.Range(0, 10) <= 5)
        {
            int lvl = MainController.Instance.PlayerData.Level;
            var slot = ShopController.RandomAfterLevelGift();
            BaseItem item = null;
            switch (slot)
            {
                case Slot.physical_weapon:
                case Slot.magic_weapon:
                case Slot.body:
                case Slot.helm:
                    item = HeroShopRandomItem.CreatMainSlot(slot, lvl);
                    break;
                case Slot.Talisman:
                    item = HeroShopRandomItem.CreaTalic(lvl);
                    break;
                case Slot.executable:
                    item = HeroShopExecutableItem.CreatExecutableItem(lvl);
                    break;
                case Slot.bonus:
                    item = HeroShopBonusItem.CreatBonusItem(lvl);
                    break;
            }
            if (item != null)
            {
                collectedItems.Add(item);
            }
        }
    }

    public DictionaryOfItemAndInt GetAllCollectedItems()
    {
        return inventory;
    }

    public void EnemieDead()
    {
        EnemiesKills++;
    }
}

