using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public enum MainParam
{
    HP = 0,
    DEF = 1,
    ATTACK = 2,
}

public class PlayerData
{
    public const string LEVEL = "LEVEL_";
    public const string ALLOCATED = "ALLOCATED_";
    public const string INVENTORY = "INVENTORY_";
    public const string ITEMS = "ITEMS";
    public const string BASE_PARAMS = "BASE_PARAMS";
    public const string BORN_POSITIONS = "BORN_POSITIONS";
    public const char ITEMS_DELEMETER = '`';
    public DictionaryOfItemAndInt playerInv = new DictionaryOfItemAndInt();
    private List<BaseItem> playerItems = new List<BaseItem>();
    public int AllocatedPoints;
    private int CurrentLevel;
    public Dictionary<MainParam,int> MainParameters;
    private readonly Dictionary<Slot,int> slotsCount = new Dictionary<Slot, int>() { {Slot.Talisman, 2}, { Slot.executable, 0 } };
    private readonly Dictionary<int, List<int>> listOfOpendBornPositions = new Dictionary<int, List<int>>(); 

    public event Action<BaseItem> OnNewItem;
    public event Action<BaseItem, bool> OnItemEquiped;
    public event Action<BaseItem> OnItemSold;
    public event Action<Dictionary<MainParam, int>> OnParametersChange;
    public event Action<int> OnLevelUp;
    public event Action<ItemId, int> OnCurrensyChanges;

    public int Level
    {
        get { return CurrentLevel; }
    }

    private int GetSlotCount(Slot s)
    {
        var c = 1;
        if (slotsCount.TryGetValue(s, out c))
        {
            return c;
        }
        return 1;
    }

    public void UpgdareParameter(MainParam parameter)
    {
        if (CanUpgradeParameter())
        {
            Debug.Log("Upgdare Main Parameter " + parameter);
            var cost = DataBaseController.Instance.DataStructs.costParameterByLvl[MainParameters[parameter]];
            //AddCurrensy(ItemId.money, -cost);
            AllocatedPoints -= 1;
            MainParameters[parameter] += 1;
            if (OnParametersChange != null)
            {
                OnParametersChange(MainParameters);
            }
            Save();
        }
    }

    public bool CanUpgradeParameter()
    {
        return (AllocatedPoints > 0);
    }

    public bool CanUpgradeLevel()
    {
        //Debug.Log("costParameterByLvl " + DataBaseController.Instance.DataStructs.costParameterByLvl.Length);
        if (DataBaseController.Instance.DataStructs.costParameterByLvl.Length > CurrentLevel)
        {
            var cost = DataBaseController.Instance.DataStructs.costParameterByLvl[CurrentLevel];
            Debug.Log("cost level " + cost);
            return CanPay(ItemId.money, cost);
        }
        return false;
    }

    public void LevelUp()
    {
        var cost = DataBaseController.Instance.DataStructs.costParameterByLvl[CurrentLevel];
        AllocatedPoints += 2;
        CurrentLevel++;
        AddCurrensy(ItemId.money, -cost);
        if (OnLevelUp != null)
        {
            OnLevelUp(CurrentLevel);
        }
    }

    public bool CanPay(ItemId t, int cost)
    {
        return cost <= playerInv[t];
    }

    public void Load()
    {
        CurrentLevel = PlayerPrefs.GetInt(LEVEL, 1);
        AllocatedPoints = PlayerPrefs.GetInt(ALLOCATED, 0);
        foreach (ItemId v in Enum.GetValues(typeof(ItemId)))
        {
            var count = PlayerPrefs.GetInt(INVENTORY + v,0);
            playerInv.Add(v,count);
        }
        var allItems = PlayerPrefs.GetString(ITEMS, "").Split(ITEMS_DELEMETER);
        foreach (var item in allItems)
        {
            if (item.Length > 4)
            {
                var fchar = item[0];
                Debug.Log("Split FC " + fchar + "   " + item);
                BaseItem itemBase = null;
                var subStr = item.Substring(1);
                switch (fchar)
                {
                    case PlayerItem.FIRSTCHAR:
                        itemBase  = PlayerItem.Creat(subStr);
                        break;
                    case BonusItem.FIRSTCHAR:
                        itemBase = BonusItem.Creat(subStr);
                        break;
                    case TalismanItem.FIRSTCHAR:
                        itemBase = TalismanItem.Creat(subStr);
                        break;
                    case ExecutableItem.FIRSTCHAR:
                        itemBase = ExecutableItem.Creat(subStr);
                        break;
                }
                if (itemBase != null)
                    playerItems.Add(itemBase);
            }
        }
        MainParameters = new Dictionary<MainParam, int>();
        var bp = PlayerPrefs.GetString(BASE_PARAMS, "");
        if (bp.Length > 3)
        {
            var allParams = bp.Split(ITEMS_DELEMETER);
            int i = 0;
            foreach (var p in allParams)
            {
                if (p.Length > 0)
                {
                    int par = Convert.ToInt32(p);
                    MainParameters.Add((MainParam) i, par);
                    i++;
                }
            }
            if (MainParameters.Count != 3)
            {
                Debug.LogError("BAD PARAMETERS LOAD " + MainParameters.Count + "   bp:" + bp);
                MainParameters.Clear();
                MainParameters.Add(global::MainParam.ATTACK, 1);
                MainParameters.Add(global::MainParam.HP, 1);
                MainParameters.Add(global::MainParam.DEF, 1);
            }
        }
        else
        {
            MainParameters.Add(global::MainParam.ATTACK, 1);
            MainParameters.Add(global::MainParam.HP, 1);
            MainParameters.Add(global::MainParam.DEF, 1);
        }
        LoadListOfBornPosition();
        CheckIfFirstLevel();
    }

    private void CheckIfFirstLevel()
    {
        var money = playerInv[ItemId.money] == 0;
        var lvl = Level == 1;
        var items = playerItems.Count == 0;
        if (money && lvl && items)
        {

            PlayerItem item1 = new PlayerItem(new Dictionary<ParamType, float>() { {ParamType.PPower, 15} },Slot.physical_weapon, false,1);
            PlayerItem item2 = new PlayerItem(new Dictionary<ParamType, float>() { { ParamType.MPower, 10 } }, Slot.magic_weapon, false, 1);
            var talisman = new TalismanItem(10,TalismanType.doubleDamage);
            var talisman2 = new TalismanItem(10, TalismanType.heal);
            item1.specialAbilities = SpecialAbility.vampire;
            AddAndEquip(item1);
            AddAndEquip(item2);
            AddAndEquip(talisman);
            AddAndEquip(talisman2);
            listOfOpendBornPositions[1].Add(1);
            foreach (var a in listOfOpendBornPositions[1])
            {
                Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>  " + a);
            }
            SaveListOfBornPosition();
        }
    }

    private void AddAndEquip(BaseItem item)
    {
        playerItems.Add(item);
        EquipItem(item);
        
    }

    public void Save()
    {
        foreach (var v in playerInv)
        {
            PlayerPrefs.SetInt(INVENTORY + v.Key.ToString(),v.Value);
        }
        string bsStr = "";
        foreach (var baseParameter in MainParameters)
        {
            bsStr += baseParameter.Value.ToString() + ITEMS_DELEMETER;
        }
        string itemsStr = "";
        foreach (var playerItem in playerItems)
        {
            itemsStr += playerItem.FirstChar() + playerItem.Save() + ITEMS_DELEMETER;
            
        }
        PlayerPrefs.SetString(ITEMS, itemsStr);
        PlayerPrefs.SetString(BASE_PARAMS, bsStr);
        PlayerPrefs.SetInt(LEVEL,CurrentLevel);
        PlayerPrefs.SetInt(ALLOCATED, AllocatedPoints);
    }
    public IEnumerable<BaseItem> GetAllWearedItems()
    {
        return playerItems.Where(x => x.IsEquped);
    }

    public void AddInventory(DictionaryOfItemAndInt inventory)
    {
        foreach (var kp in inventory)
        {
            AddCurrensy(kp.Key,kp.Value);
        }
    }

    public void AddCurrensy(ItemId id, int count)
    {
        playerInv[id] += count;
        if (OnCurrensyChanges != null)
        {
            OnCurrensyChanges(id, playerInv[id]);
        }
        Save();

    }

    public void AddItem(BaseItem item,bool withSave = true)
    {
        if (OnNewItem != null)
        {
            OnNewItem(item);
        }
        playerItems.Add(item);
        if (withSave)
            Save();
    }

    public List<BaseItem> GetAllItems()
    {
        return playerItems;
    }

    public void EquipItem(BaseItem playerItem,bool equip = true)
    {
        PlayerItem oldEquipedItem = null;
        if (equip)
        {
            if (playerItem.IsEquped)
                return;

            var oldEquipedItems = playerItems.Where(x => x.IsEquped && x.Slot == playerItem.Slot);
            var slotCount = GetSlotCount(playerItem.Slot);
            if (oldEquipedItems.Count() >= slotCount)
            {
                var l = oldEquipedItems.ToList();
                var item2Unquip = l[0];
                item2Unquip.IsEquped = false;
                if (OnItemEquiped != null)
                {
                    OnItemEquiped(item2Unquip, false);
                }

            }
            playerItem.IsEquped = true;
            if (OnItemEquiped != null)
            {
                OnItemEquiped(playerItem, true);
            }

        }
        else
        {
            playerItem.IsEquped = false;
            if (OnItemEquiped != null)
            {
                OnItemEquiped(playerItem, false);
            }

        }
        Save();
    }

    public void Sell(BaseItem playerItem)
    {
        AddCurrensy(ItemId.money, playerItem.cost/3);
        playerItems.Remove(playerItem);
        playerItem.IsEquped = false;
        if (OnItemSold != null)
        {
            OnItemSold(playerItem);
        }
        Save();
    }

    public float CalcParameter(ParamType type)
    {
        float v = 0;
        foreach (PlayerItem playerItem in playerItems.Where(x=>x.IsEquped &&  x is PlayerItem && (x as PlayerItem).parameters.ContainsKey(type)))
        {
            foreach (var parameter in playerItem.parameters)
            {
                if (parameter.Key == type)
                {
                    v += parameter.Value;
                }
            }
        }
        switch (type)
        {
            case ParamType.Speed:
                v += 4;
                break;
            case ParamType.MPower:
                v += MainParameters[MainParam.ATTACK] * 12 + 36;
                break;
            case ParamType.PPower:
                v += MainParameters[MainParam.ATTACK] * 10 + 32;
                break;
            case ParamType.PDef:
                v += MainParameters[MainParam.DEF] * 10 + 10;
                break;
            case ParamType.MDef:
                v += MainParameters[MainParam.DEF] * 9;
                break;
            case ParamType.Hp:
                v += MainParameters[MainParam.HP] * 40 + 200;
                break;
        }
        return v;
    }

    public void Pay(ItemId itemId, int cost)
    {
        AddCurrensy(itemId, -cost);
    }

    public void OpenBornPosition(int id)
    {
        var a = MainController.Instance.level.MissionIndex;
        listOfOpendBornPositions[a].ForceAddValue(id);
        SaveListOfBornPosition();
    }

    private void SaveListOfBornPosition()
    {
        string save_string;
        foreach (var bornPosition in listOfOpendBornPositions)
        {
            save_string = "";
            foreach (var val in bornPosition.Value)
            {
                save_string += val.ToString() + ITEMS_DELEMETER;
            }
            var saveKey = BORN_POSITIONS + bornPosition.Key.ToString();
//            Debug.Log("SaveListOfBornPosition:" + saveKey + "    " + save_string);
            PlayerPrefs.SetString(saveKey, save_string);
        }
    }

    private void LoadListOfBornPosition()
    {
        for (int i = 1; i < DataStructs.MISSION_LAST_INDEX + 1; i++)
        {
            List<int> list = new List<int>();
            listOfOpendBornPositions.Add(i, list);
            var str = PlayerPrefs.GetString(BORN_POSITIONS + i,"");
//            Debug.Log(i + " LoadListOfBornPosition " + str + "  " + str.Length);
            if (str.Length > 0)
            {
                var splited = str.Split(ITEMS_DELEMETER);
                foreach (var s in splited)
                {
                    if (s.Length > 0)
                    {
                        list.Add(Convert.ToInt32(s));
                    }
                }
            }
        }
    }

    public List<int> GetAllBornPositions(int mission)
    {
        return listOfOpendBornPositions[mission];
    }

    internal bool IsPositionOpen(int misson,int index)
    {
        return listOfOpendBornPositions[misson].Contains(index);
    }

    public void RemoveItem(BaseItem bonusItem)
    {
        playerItems.Remove(bonusItem);
    }
}

