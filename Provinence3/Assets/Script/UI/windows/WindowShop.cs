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
    public UpgradeWindow UpgradeWindow;
    private Bookmarks Bookmarks;

    public override void Init()
    {
        base.Init();
        NullSelection();
        ItemInfoElement.SetCallBack(OnItemInit);
        AllParametersContainer.Init();
        moneyField.text = MainController.Instance.PlayerData.playerInv[ItemId.money].ToString("0");
        crystalField.text = MainController.Instance.PlayerData.playerInv[ItemId.crystal].ToString("0");
        InitPlayerItems();
        OnMarkerChange();
        
        MainController.Instance.PlayerData.OnNewItem += OnNewItem;
        MainController.Instance.PlayerData.OnItemEquiped += OnItemEquipedCallback;
        MainController.Instance.PlayerData.OnItemSold += OnItemSoldCallback;
        MainController.Instance.PlayerData.OnCurrensyChanges += OnCurrensyChanges;
        MainController.Instance.PlayerData.OnChangeCount += OnChangeCount;
    }


    private void InitPlayerItems()
    {
        PlayerItemElements = new List<PlayerItemElement>();
        List<BaseItem> items = MainController.Instance.PlayerData.GetAllItems();
        Debug.Log("items count = " + items.Count);
        foreach (var playerItem in items)
        {
            var element = DataBaseController.GetItem<PlayerItemElement>(PrefabPlayerItemElement);
            element.Init(playerItem, OnSelected);
            element.transform.SetParent(layoutMyInventory, false);
            PlayerItemElements.Add(element);
        }
    }

    public void InitGoods()
    {
        ClearTransform(layoutShopItems);
        int lvl = MainController.Instance.PlayerData.Level;
        for (int i = Mathf.Clamp(lvl - 2,1,Int32.MaxValue); i <= lvl ; i++)
        {
            CreatShopElement(new HeroShopRandomItem(i));
        }
        CreatShopElement(new HeroShopBonusItem(lvl));
        CreatShopElement(new HeroShopExecutableItem(lvl));
        Bookmarks = Bookmarks.weapons;
    }

    public void InitRecipies()
    {
        ClearTransform(layoutShopItems);
        int lvl = MainController.Instance.PlayerData.Level;
        for (int i = Mathf.Clamp(lvl - 1, 1, Int32.MaxValue); i <= lvl; i++)
        {
            CreatShopElement(new HeroShopRecipeItem(i));
        }
        Bookmarks = Bookmarks.recipies;
    }
    public void OnMarkerChange()
    {
        switch (Bookmarks)
        {
            case Bookmarks.weapons:
                InitGoods();
                break;
            case Bookmarks.recipies:
                InitRecipies();
                break;
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
            if (info.Slot != Slot.executable)
            {

                EquipButton.gameObject.SetActive(!isEquiped);
                UnEquipButton.gameObject.SetActive(isEquiped);
                var canBeupgraded = MainController.Instance.PlayerData.CanBeUpgraded(info) != null;
                UpgradeButton.gameObject.SetActive(canBeupgraded);
            }
        }

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
        if (selectedShopElement != null && selectedShopElement.CanBuy && EnoughtMoney(selectedShopElement))
        {
            WindowManager.Instance.ConfirmWindow.Init(
                () => { ShopController.Instance.BuyItem(selectedShopElement); }
                ,null,"Do u what to but it?");
        }
        else
        {
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
            if (exec.count >= 1)
            {
                item.Refresh();
            }
            else
            {
                Destroy(item.gameObject);
            
            }
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
        PlayerItemElements.Add(element);
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

