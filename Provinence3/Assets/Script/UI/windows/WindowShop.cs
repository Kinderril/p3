using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum Bookmarks
{
    weapons,
    recipies,
}

public class WindowShop : BaseWindow
{

    public Transform layoutMyInventory;
    public Transform layoutShopItems;
    public PlayerItemElement PrefabPlayerItemElement;
    public ShopItemElement PrefabShopItemElement;
    public ItemInfoElement ItemInfoElement;
    private List<PlayerItemElement> PlayerItemElements;
    public Text moneyField;
    public Text crystalField;
    private IShopExecute selectedShopElement;
    private BaseItem selectedPlayerItem;
    public AllParametersContainer AllParametersContainer;
//    public Button BuyButton;
//    public Button SellButton;
//    public Button EquipButton;
//    public Button UnEquipButton;
//    public Button UpgradeButton;
//    public Button RecipeButton;
    public ItemWindow ItemWindow;
    private Bookmarks Bookmarks = Bookmarks.recipies;
    public UpgradeWindow UpgradeWindow;
    public CraftWindow CraftWindow;

    public HeroShopExecutableItem PrefabHeroShopExecutableItem;
    public HeroShopBonusItem PrefabHeroShopBonusItem;
//    public HeroShopRandomItem PrefabHeroShopRandomItem;
    public HeroShopWeapon HeroShopWeapon;
    public HeroShopTalisman HeroShopTalisman;
    public HeroShopArmor HeroShopArmor;
    public HeroShopRecipeItem PrefabHeroShopRecipeItem;
    public HeroShopCheat PrefabHeroShopCheat;

    public override void Init()
    {
        base.Init();
        UpgradeWindow.gameObject.SetActive(false);
        CraftWindow.gameObject.SetActive(false);
        ItemWindow.gameObject.SetActive(false);
//        ItemInfoElement.SetCallBack(OnItemInit);
        AllParametersContainer.Init();
        moneyField.text = MainController.Instance.PlayerData.playerInv[ItemId.money].ToString("0");
        crystalField.text = MainController.Instance.PlayerData.playerInv[ItemId.crystal].ToString("0");
        InitPlayerItems();
        InitGoods();
        NullSelection();

        MainController.Instance.PlayerData.OnNewItem += OnNewItem;
        MainController.Instance.PlayerData.OnItemEquiped += OnItemEquipedCallback;
        MainController.Instance.PlayerData.OnItemSold += OnItemSoldCallback;
        MainController.Instance.PlayerData.OnCurrensyChanges += OnCurrensyChanges;
        MainController.Instance.PlayerData.OnChangeCount += OnChangeCount;
        MainController.Instance.PlayerData.OnEnchant += OnEnchant;
    }

    private void OnEnchant(IEnhcant arg1, bool arg2)
    {
        var msg = arg2 ? "Item was succsesfully enchant" : "Enchant failed";
        WindowManager.Instance.InfoWindow.Init(() => { }, msg);
    }

    private void Sort()
    {
        PlayerItemElements.Sort((x, y) =>
        {
            var xPriority = GetPriority(x);
            var yPriority = GetPriority(y);
            if (xPriority > yPriority)
            {
                return 1;
            }

            if (yPriority > xPriority)
            {
                return -1;
            }
            return 0;
        });
        for (int i = PlayerItemElements.Count - 1; i >=0 ; i--)
        {
            var pe = PlayerItemElements[i];
            pe.transform.SetAsLastSibling();
        }
    }

    private int GetPriority(PlayerItemElement p0)
    {

        switch (p0.PlayerItem.Slot)
        {
            case Slot.physical_weapon:
            case Slot.magic_weapon:
                return 10 + p0.PlayerItem.Id;
            case Slot.body:
                return 8;
            case Slot.helm:
                return 8;
            case Slot.Talisman:
                return 7;
            case Slot.bonus:
                return 6;
            case Slot.recipe:
                return 5;
            case Slot.executable:
                if (p0.PlayerItem is ExecEnchantItem)
                    return 4;
                return 3;
        }
        return 0;

    }

    private void InitPlayerItems()
    {
        PlayerItemElements = new List<PlayerItemElement>();
        List<BaseItem> items = MainController.Instance.PlayerData.GetAllItems();
//        Debug.Log("items count = " + items.Count);
        foreach (var playerItem in items)
        {
            var element = DataBaseController.GetItem<PlayerItemElement>(PrefabPlayerItemElement);
            element.Init(playerItem, OnSelected);
            element.transform.SetParent(layoutMyInventory, false);
            AddNewItem(element);
        }
    }

    private void AddNewItem(PlayerItemElement element)
    {
        PlayerItemElements.Add(element);
        Sort();
    }

    public void InitGoods()
    {
        ClearTransform(layoutShopItems);
        int lvl = MainController.Instance.PlayerData.Level;
        var countShopItems = Mathf.Clamp(lvl - 2, 1, Int32.MaxValue);
        for (int i = countShopItems; i <= lvl ; i++)
        {
            var e = DataBaseController.GetItem<HeroShopRandomItem>(HeroShopWeapon);
            e.Init(i);
            CreatShopElement(e);
            var e2 = DataBaseController.GetItem<HeroShopRandomItem>(HeroShopArmor);
            e2.Init(i);
            CreatShopElement(e2);
            var e3 = DataBaseController.GetItem<HeroShopRandomItem>(HeroShopTalisman);
            e3.Init(i);
            CreatShopElement(e3);
#if DEBUG
            var e4 = DataBaseController.GetItem<HeroShopCheat>(PrefabHeroShopCheat);
            e4.Init(i);
            CreatShopElement(e4);
#endif
        }


        var bonItem = DataBaseController.GetItem<HeroShopBonusItem>(PrefabHeroShopBonusItem);
        bonItem.Init(lvl);
        CreatShopElement(bonItem);

        var execItem = DataBaseController.GetItem<HeroShopExecutableItem>(PrefabHeroShopExecutableItem);
        execItem.Init(lvl);
        CreatShopElement(execItem);
        Bookmarks = Bookmarks.weapons;
        NullSelection();
    }

    public void InitRecipies()
    {
        ClearTransform(layoutShopItems);
        int lvl = MainController.Instance.PlayerData.Level;
        for (int i = Mathf.Clamp(lvl - 1, 1, Int32.MaxValue); i <= lvl; i++)
        {
            var execItem = DataBaseController.GetItem<HeroShopRecipeItem>(PrefabHeroShopRecipeItem);
            execItem.Init(lvl);
            CreatShopElement(execItem);
        }
        Bookmarks = Bookmarks.recipies;
        NullSelection();
    }


    private void CreatShopElement(IShopExecute exec)
    {
        var element = DataBaseController.GetItem<ShopItemElement>(PrefabShopItemElement);
        element.Init(exec, OnShopSelected);
        exec.transform.SetParent(element.transform,false);
        element.transform.SetParent(layoutShopItems, false);
    }

    private void NullSelection()
    {
        selectedShopElement = null;
        selectedPlayerItem = null;
        
//        Debug.Log("child count " + layoutMyInventory.childCount);
//        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutMyInventory.GetComponent<RectTransform>());
//        if (layoutMyInventory.childCount > 0)
//        {
//            var tmp = layoutMyInventory.GetChild(0);
//            var shopExec = tmp.GetComponent<PlayerItemElement>();
//            if (shopExec != null)
//                OnSelected(shopExec);
//        }
        StartCoroutine(WaitBeforSelect());
    }

    private IEnumerator WaitBeforSelect()
    {
        yield return new WaitForEndOfFrame();
        if (layoutMyInventory.childCount > 0)
        {
            var tmp = layoutMyInventory.GetChild(0);
            var shopExec = tmp.GetComponent<PlayerItemElement>();
            if (shopExec != null)
                OnSelected(shopExec);
        }
    }

    private void OnChangeCount(ExecutableItem obj,int delta)
    {
        var element = PlayerItemElements.FirstOrDefault(x => x.PlayerItem == obj);
        if (element != null)
        {
            element.Refresh();
        }
        if (delta > 0)
            ItemWindow.Init(obj);
    }

    public void RefreshItem(BaseItem item)
    {
        var element = PlayerItemElements.FirstOrDefault(x => x.PlayerItem == item);
        if (element != null)
        {
            element.Refresh();
            OnSelected(element);
        }
    } 
    
    private void OnShopSelected(IShopExecute obj)
    {
        selectedShopElement = obj;
        ItemInfoElement.gameObject.SetActive(true);
        ItemInfoElement.Init(selectedShopElement);
    }

    private void OnCurrensyChanges(ItemId arg1, int arg2)
    {
        switch (arg1)
        {
            case ItemId.money:
                moneyField.text = arg2.ToString("0");
                break;
            case ItemId.crystal:
                crystalField.text = arg2.ToString("0");
                break;
        }
    }

    private void OnItemSoldCallback(BaseItem obj)
    {
        Debug.Log("OnItemSold");
        var exec = obj as ExecutableItem;
        var item = PlayerItemElements.FirstOrDefault(x => x.PlayerItem == obj);
        if (item != null)
        {
            
            if (exec != null && exec.count >= 1)
            {
                item.Refresh();
            }
            else
            {
                Destroy(item.gameObject);

            }
            PlayerItemElements.Remove(item);
            Sort();
        }
        NullSelection();
    }




    private void OnItemEquipedCallback(BaseItem obj,bool val)
    {
        Debug.Log("OnItemEquiped");
        var item = PlayerItemElements.FirstOrDefault(x => x.PlayerItem == obj);
        if (item != null)
        {
            item.Equip(val);
        }
        AllParametersContainer.UpgradeValues();
    }

    public void OnSelected(PlayerItemElement item)
    {
        selectedPlayerItem = item.PlayerItem;
        ItemInfoElement.Init(selectedPlayerItem);
    }

    private void OnNewItem(BaseItem playerItem)
    {
        ItemWindow.Init(playerItem);
        //WindowManager.Instance.InfoWindow.Init(null,"you buy new item");
        var element = DataBaseController.GetItem<PlayerItemElement>(PrefabPlayerItemElement);
        element.Init(playerItem, OnSelected);
        element.transform.SetParent(layoutMyInventory,false);
        AddNewItem(element);
    }

    public override void Close()
    {
        MainController.Instance.PlayerData.OnChangeCount -= OnChangeCount;
        MainController.Instance.PlayerData.OnNewItem -= OnNewItem;
        MainController.Instance.PlayerData.OnItemEquiped -= OnItemEquipedCallback;
        MainController.Instance.PlayerData.OnItemSold -= OnItemSoldCallback;
        MainController.Instance.PlayerData.OnEnchant -= OnEnchant;
        MainController.Instance.PlayerData.OnCurrensyChanges -= OnCurrensyChanges;
        PlayerItemElements.Clear();
        ClearTransform(layoutMyInventory);
        ClearTransform(layoutShopItems);
        base.Close();
    }

    public void OpenCraft(RecipeItem recipe)
    {
        CraftWindow.Init(recipe, OnCraftComplete);
    }

    private void OnCraftComplete(BaseItem obj)
    {
        ItemWindow.Init(obj);
    }
}

