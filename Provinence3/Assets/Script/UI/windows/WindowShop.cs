using System;
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
    public Button BuyButton;
    public Button SellButton;
    public Button EquipButton;
    public Button UnEquipButton;
    public Button UpgradeButton;
    public Button RecipeButton;
    public UpgradeWindow UpgradeWindow;
    public CraftWindow CraftWindow;
    private Bookmarks Bookmarks = Bookmarks.recipies;

    public HeroShopExecutableItem PrefabHeroShopExecutableItem;
    public HeroShopBonusItem PrefabHeroShopBonusItem;
    public HeroShopRandomItem PrefabHeroShopRandomItem;
    public HeroShopRecipeItem PrefabHeroShopRecipeItem;

    public override void Init()
    {
        base.Init();
        NullSelection();
        CraftWindow.gameObject.SetActive(false);
        ItemInfoElement.SetCallBack(OnItemInit);
        AllParametersContainer.Init();
        moneyField.text = MainController.Instance.PlayerData.playerInv[ItemId.money].ToString("0");
        crystalField.text = MainController.Instance.PlayerData.playerInv[ItemId.crystal].ToString("0");
        InitPlayerItems();
        InitGoods();
        
        MainController.Instance.PlayerData.OnNewItem += OnNewItem;
        MainController.Instance.PlayerData.OnItemEquiped += OnItemEquipedCallback;
        MainController.Instance.PlayerData.OnItemSold += OnItemSoldCallback;
        MainController.Instance.PlayerData.OnCurrensyChanges += OnCurrensyChanges;
        MainController.Instance.PlayerData.OnChangeCount += OnChangeCount;
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
                return 10;
            case Slot.magic_weapon:
                return 10;
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
        for (int i = Mathf.Clamp(lvl - 2,1,Int32.MaxValue); i <= lvl ; i++)
        {
            var e = DataBaseController.GetItem<HeroShopRandomItem>(PrefabHeroShopRandomItem);
            e.Init(i);
//            var e = new HeroShopRandomItem(i);
            CreatShopElement(e);
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

    public void OnRecipeOpen()
    {
        var recipe = selectedPlayerItem as RecipeItem;
        if (recipe != null)
        {
            CraftWindow.Init(recipe);
        }
    }

    private void CreatShopElement(IShopExecute exec)
    {
        var element = DataBaseController.GetItem<ShopItemElement>(PrefabShopItemElement);
        element.Init(exec, OnShopSelected);
        element.transform.SetParent(layoutShopItems, false);
    }

    private void NullSelection()
    {

        ItemInfoElement.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        UnEquipButton.gameObject.SetActive(false);
        UpgradeButton.gameObject.SetActive(false);
        BuyButton.gameObject.SetActive(false);
        SellButton.gameObject.SetActive(false);
        selectedShopElement = null;
        selectedPlayerItem = null;
    }

    private void OnItemInit(BaseItem info,ItemOwner obj)
    {
        bool val = info == null;
        BuyButton.gameObject.SetActive(val);
        EquipButton.gameObject.SetActive(false);
        UnEquipButton.gameObject.SetActive(false);
        UpgradeButton.gameObject.SetActive(false);
        if (!val)
        {
            bool isEquiped = info.IsEquped;
            if (info.Slot != Slot.executable && info.Slot != Slot.recipe)
            {
                EquipButton.gameObject.SetActive(!isEquiped);
                UnEquipButton.gameObject.SetActive(isEquiped);
                var canBeupgraded = MainController.Instance.PlayerData.CanBeUpgraded(info) != null;
                UpgradeButton.gameObject.SetActive(canBeupgraded);
            }
        }
        RecipeButton.gameObject.SetActive(info is RecipeItem);

        SellButton.gameObject.SetActive(!val);
        ItemInfoElement.gameObject.SetActive(true);
    }
    private void OnChangeCount(ExecutableItem obj)
    {
        var element = PlayerItemElements.FirstOrDefault(x => x.PlayerItem == obj);
        if (element != null)
        {
            element.Refresh();
        }
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

    public void OnUpgradeClick()
    {
        var playerItem = selectedPlayerItem as PlayerItem;
        if (playerItem != null)
        {
            UpgradeWindow.Init(playerItem, OnItemEnchanted);
        }
        else
        {
            WindowManager.Instance.InfoWindow.Init(null, "Can't Enchant this item");
        }
    }

    private void OnItemEnchanted(PlayerItem obj)
    {
        RefreshItem(obj);
    }

    public void OnBuySimpleChest()
    {
        if (/*selectedShopElement != null && */selectedShopElement.CanBuy && EnoughtMoney(selectedShopElement))
        {
            WindowManager.Instance.ConfirmWindow.Init(
                () => { ShopController.Instance.BuyItem(selectedShopElement); }
                ,null,"Do u what to but it?");
        }
        else
        {
//            if (selectedShopElement == null)
//            {
//                Debug.Log("selectedShopElement : " + selectedShopElement.MoneyCost);
//            }
//
//            Debug.Log("selectedShopElement != null " + (selectedShopElement == null) + "   " + selectedShopElement.MoneyCost);
            WindowManager.Instance.InfoWindow.Init(null,"not enought money");
        }
    }

    public void OnUnequipItem()
    {
        if (selectedPlayerItem != null)
        {
            MainController.Instance.PlayerData.EquipItem(selectedPlayerItem,false);
        }
    }

    private bool EnoughtMoney(IShopExecute selectedShopElement)
    {
        bool haveMoney = MainController.Instance.PlayerData.CanPay(ItemId.money,selectedShopElement.MoneyCost);
        bool haveCrystal = MainController.Instance.PlayerData.CanPay(ItemId.crystal, selectedShopElement.CrystalCost);
        return haveMoney && haveCrystal;
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

    public void OnEquip()
    {
        if (selectedPlayerItem == null)
        {
            Debug.Log("no one selected");
            return;
        }
        MainController.Instance.PlayerData.EquipItem(selectedPlayerItem);
    }


    public void OnSell()
    {
        WindowManager.Instance.ConfirmWindow.Init(() => MainController.Instance.PlayerData.Sell(selectedPlayerItem),
            null, "do you wnat to sell it?");
    }

    private void OnItemEquipedCallback(BaseItem obj,bool val)
    {
        Debug.Log("OnItemEquiped");
        var item = PlayerItemElements.FirstOrDefault(x => x.PlayerItem == obj);
        if (item != null)
        {
            item.Equip(val);
            EquipButton.gameObject.SetActive(!val);
            UnEquipButton.gameObject.SetActive(val);

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
        MainController.Instance.PlayerData.OnCurrensyChanges -= OnCurrensyChanges;
        PlayerItemElements.Clear();
        ClearTransform(layoutMyInventory);
        ClearTransform(layoutShopItems);
        base.Close();
    }
}

