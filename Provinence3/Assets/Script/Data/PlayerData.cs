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
    public const int CRYSTAL_SAFETY_ENCHANT = 10;
    public const int ENCHANT_CHANCE = 60;
    public const int POINTS_PER_LVL = 2;
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
    private readonly Dictionary<Slot,int> slotsCount = new Dictionary<Slot, int>()
    {
        {Slot.Talisman, 2}, { Slot.executable, 0 } , {Slot.bonus, 5 }
    };
    public OpenLevels OpenLevels;

    public event Action<BaseItem> OnNewItem;
    public event Action<ExecutableItem,int> OnChangeCount;
    public event Action<BaseItem, bool> OnItemEquiped;
    public event Action<BaseItem> OnItemSold;
    public event Action<Dictionary<MainParam, int>> OnParametersChange;
    public event Action<int> OnLevelUp;
    public event Action<ItemId, int> OnCurrensyChanges;
    public event Action<IEnhcant, bool> OnEnchant;

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
//            var cost = Formuls.LevelUpCost(MainParameters[parameter]);
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

    public void TryToEnchant(IEnhcant item,bool safety)
    {
        var exec = CanBeUpgraded(item);
        if (exec != null)
        {
            if (safety)
            {
                if (CanPay(ItemId.crystal, CRYSTAL_SAFETY_ENCHANT))
                {
                    DoEnchant(item, exec, safety);
                }
            }
            else
            {
                DoEnchant(item, exec, safety);
            }
        }
    }

    private void DoEnchant(IEnhcant item, ExecutableItem exec, bool safety)
    {
        RemoveItem(exec);
        if (safety)
        {
            Pay(ItemId.crystal, CRYSTAL_SAFETY_ENCHANT);
        }
        var enchantFine = UnityEngine.Random.Range(0, 100) < ENCHANT_CHANCE;
        Debug.Log("Enchant done:" + enchantFine);
        if (enchantFine)
        {
            item.Enchant(1);
        }
        else
        {
            if (safety)
            {
                item.Enchant(0);
            }
            else
            {
                item.Enchant(-5);
            }
        }
        Save();
        if (OnEnchant != null)
        {
            OnEnchant(item, enchantFine);
        }
    }

    public bool CanUpgradeParameter()
    {
        return (AllocatedPoints > 0);
    }

    public bool CanUpgradeLevel()
    {
        //Debug.Log("costParameterByLvl " + DataBaseController.Instance.DataStructs.costParameterByLvl.Length);
        var cost = Formuls.LevelUpCost(CurrentLevel);
        Debug.Log("cost level " + cost);
        return CanPay(ItemId.money, cost);
    }

    public void LevelUp()
    {
        var cost = Formuls.LevelUpCost(CurrentLevel);
        AllocatedPoints += POINTS_PER_LVL;
        CurrentLevel++;
        AddCurrensy(ItemId.money, -cost);
        Save();
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
            if (item.Length > 2)
            {
                var fchar = item[0];
                Debug.Log("Split FC " + fchar + "   " + item);
                BaseItem itemBase = null;
                var subStr = item.Substring(1);
                switch (fchar)
                {
                    case PlayerItem.FIRSTCHAR:
                        itemBase  = PlayerItem.Create(subStr);
                        break;
                    case BonusItem.FIRSTCHAR:
                        itemBase = BonusItem.Create(subStr);
                        break;
                    case TalismanItem.FIRSTCHAR:
                        itemBase = TalismanItem.Create(subStr);
                        break;
                    case ExecutableItem.FIRSTCHAR:
                        itemBase = ExecutableItem.Create(subStr);
                        break;
                    case RecipeItem.FIRSTCHAR:
                        itemBase = RecipeItem.Create(subStr);
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
//                Debug.LogError("BAD PARAMETERS LOAD " + MainParameters.Count + "   bp:" + bp);
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
        OpenLevels = new OpenLevels();
        CheckIfFirstLevel();
#if UNITY_EDITOR
        if (DebugController.Instance.GET_START_BOOST)
        {
            playerInv[ItemId.money] += 1000;
            playerInv[ItemId.crystal] += 10;
        }
#endif
    }
    public ExecutableItem CanBeUpgraded(IEnhcant info)
    {
        switch (info.BaseItem.Slot)
        {
            case Slot.physical_weapon:
            case Slot.magic_weapon:
                return HaveExecutableItem(EnchantType.weaponUpdate);
            case Slot.body:
            case Slot.helm:
                return HaveExecutableItem(EnchantType.armorUpdate);
            case Slot.Talisman:
                return HaveExecutableItem(EnchantType.powerUpdate);
        }
        return null;
    }
    
    private ExecutableItem HaveExecutableItem(EnchantType t)
    {
        var allItems = GetAllItems();
        return allItems.FirstOrDefault(x => x.Slot == Slot.executable 
        && ((ExecutableItem)x).ExecutableType == ExecutableType.enchant 
        && ((ExecEnchantItem)x).ItemType == t
        ) as ExecutableItem;
    }

    private void CheckIfFirstLevel()
    {
        var money = playerInv[ItemId.money] == 0;
        var lvl = Level == 1;
        var items = playerItems.Count == 0;
        if (money && lvl && items)
        {
            var p1 = Formuls.GetPlayerItemPointsByLvl(1);
            PlayerItem item1 = new PlayerItem(new Dictionary<ParamType, float>() { {ParamType.PPower, p1 } },Slot.physical_weapon, Rarity.Normal, p1);
            var p2 = Formuls.GetPlayerItemPointsByLvl(1);
            PlayerItem item2 = new PlayerItem(new Dictionary<ParamType, float>() { { ParamType.MPower, p2 } }, Slot.magic_weapon, Rarity.Normal, p2);
            AddAndEquip(item1);
            AddAndEquip(item2);
            AddFirstTalisman(TalismanType.doubleDamage);
            AddFirstTalisman(TalismanType.heal);

#if UNITY_EDITOR
            if (DebugController.Instance.GET_START_BOOST)
            {
                PlayerItem item3 = new PlayerItem(new Dictionary<ParamType, float>() { { ParamType.PDef, 25 } }, Slot.body, Rarity.Rare, 25);
                AddAndEquip(item3);

                foreach (var ability in ShopController.AllTalismanstypes)
                {
                    AddFirstTalisman(ability);
                }
            }
            if (DebugController.Instance.GET_ALL_TYPE_WEAPONS_BOOST)
            {
                int i = 0;
                var baseP = Formuls.GetPlayerItemPointsByLvl(1) + 1;
                foreach (var ability in ShopController.AllSpecialAbilities)
                {
                    var y = baseP + i++;
                    PlayerItem itemA = new PlayerItem(new Dictionary<ParamType, float>() { { ParamType.PPower, y } }, Slot.physical_weapon, Rarity.Normal, y);
                    itemA.specialAbilities = ability;
                    AddAndEquip(itemA);
                }
                
            }


#endif

            }
    }

    private void AddFirstTalisman(TalismanType t)
    {
        var talisman2 = new TalismanItem(40, t);
        AddAndEquip(talisman2);
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
    }

    public void AddItem(BaseItem item,bool withSave = true)
    {
        var executable = item as ExecutableItem;
        if (executable != null)
        {
            ExecutableItem oldItem = null;
            switch (executable.ExecutableType)
            {
                case ExecutableType.craft:
                    var orevType = (executable as ExecCraftItem).ItemType;
                    oldItem =
                        playerItems.FirstOrDefault(
                            x => x is ExecCraftItem 
                            && (x as ExecCraftItem).ItemType == orevType) as ExecutableItem;
                    break;
                case ExecutableType.enchant:
                    var orevTypeE = (executable as ExecEnchantItem).ItemType;
                    oldItem =
                        playerItems.FirstOrDefault(
                            x => x is ExecEnchantItem
                            && (x as ExecEnchantItem).ItemType == orevTypeE) as ExecutableItem;
                break;
                case ExecutableType.catalys:
                    var orevTypeC = (executable as ExecCatalysItem).ItemType;
                    oldItem =
                        playerItems.FirstOrDefault(
                            x => x is ExecCatalysItem
                            && (x as ExecCatalysItem).ItemType == orevTypeC) as ExecutableItem;
                break;
            }
            if (oldItem != null)
            {
                (oldItem).count += executable.count;
                if (OnChangeCount != null)
                {
                    OnChangeCount(oldItem, executable.count);
                }
            }
            else
            {
                if (OnNewItem != null)
                {
                    OnNewItem(item);
                }
                playerItems.Add(item);
            }
        }
        else
        {
            if (OnNewItem != null)
            {
                OnNewItem(item);
            }
            playerItems.Add(item);
        }
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
        AddCurrensy(ItemId.money, playerItem.cost);
        RemoveItem(playerItem);
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
        return Formuls.AffectMainParam(v, MainParameters, type);
    }

    public void Pay(ItemId itemId, int cost)
    {
        AddCurrensy(itemId, -cost);
    }

    public void OpenBornPosition(int id)
    {
        OpenLevels.OpenPosition(MainController.Instance.level.MissionIndex, id);
    }

    private IEnumerable<T> PreFilter<T>(ExecutableItem item) where T: ExecutableItem
    {
        var preFilter = playerItems.Where(x => x is T
                   && ((T) x).ExecutableType == item.ExecutableType);
        return preFilter.Select(baseItem => baseItem as T).ToList();
    }

    public void RemoveExecutableItem(ExecutableItem item, int count)
    {
        ExecutableItem oldItem = null;
        switch (item.ExecutableType)
        {
            case ExecutableType.craft:
                var p1 = PreFilter<ExecCraftItem>(item);
                oldItem = p1.FirstOrDefault(x => x.ItemType == (item as ExecCraftItem).ItemType);
                break;
            case ExecutableType.enchant:
                var p2 = PreFilter<ExecEnchantItem>(item);
                oldItem = p2.FirstOrDefault(x => x.ItemType == (item as ExecEnchantItem).ItemType);
                break;
            case ExecutableType.catalys:
                var p3 = PreFilter<ExecCatalysItem>(item);
                oldItem = p3.FirstOrDefault(x => x.ItemType == (item as ExecCatalysItem).ItemType);
                break;
        }
        if (oldItem == null)
        {
            Debug.LogError("can't Sell item " + item);
            return;
        }
        

        if (oldItem.count >= count)
        {
            oldItem.count -= count;
            if (oldItem.count == 0)
            {
                playerItems.Remove(oldItem);
                if (OnItemSold != null)
                {
                    OnItemSold(oldItem);
                }
            }
            else
            {
                if (OnChangeCount != null)
                {
                    OnChangeCount(oldItem,-count);
                }
            }
        }
    }

    public void RemoveItem(BaseItem item,int count = 1)
    {
        if (count == 0)
            return;
        var executable = item as ExecutableItem;
        if (executable != null)
        {
            RemoveExecutableItem(executable, count);
            return;
        }
        playerItems.Remove(item);
        item.Clear();
        item.IsEquped = false;

        if (OnItemSold != null)
        {
            OnItemSold(item);
        }
    }

    public PlayerItem DoCraft(RecipeItem recipeItem, ExecCatalysItem catalysItem = null)
    {
        foreach (var execCraftItem in recipeItem.ItemsToCraft())
        {
            RemoveItem(execCraftItem, execCraftItem.count);
        }
        var resultItem = HeroShopRandomItem.CreatMainSlot(recipeItem.recipeSlot, recipeItem.Level, catalysItem);
        AddItem(resultItem);
        Save();
        return resultItem;
    }
}


