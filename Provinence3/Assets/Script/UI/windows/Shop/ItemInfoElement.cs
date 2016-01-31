using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum ItemOwner
{
    Shop,
    Player
}
public class ItemInfoElement : MonoBehaviour
{
    public ParameterElement Prefab;
    public Text NameLabel;
    public Transform layout;
    public Transform moneyLayout;
    public Image SlotLabel;
    public Image mainIcon;
    public Image SpecIcon;
    public Action<BaseItem, ItemOwner> OnInitCallback;

    public void SetCallBack(Action<BaseItem, ItemOwner> OnInitCallback)
    {
        this.OnInitCallback = OnInitCallback;
    }


    public void Init(BaseItem item)
    {
        Clear();
        SlotLabel.sprite = DataBaseController.Instance.SlotIcon(item.Slot);
        SpecIcon.gameObject.SetActive(false);
        var playerItem = item as PlayerItem;
        if (playerItem != null)
        {
            foreach (var p in playerItem.parameters)
            {
                var element = DataBaseController.GetItem<ParameterElement>(Prefab);
                element.Init(p.Key, p.Value);
                element.transform.SetParent(layout);
            }
            var haveSpec = playerItem.specialAbilities != SpecialAbility.none;
            SpecIcon.gameObject.SetActive(haveSpec);
            if (haveSpec)
            {
                SpecIcon.gameObject.SetActive(true);
                SpecIcon.sprite = DataBaseController.Instance.SpecialAbilityIcon(playerItem.specialAbilities);
            }
            mainIcon.sprite = Resources.Load<Sprite>("sprites/PlayerItems/" + playerItem.icon);
            NameLabel.text = playerItem.name;
        }
        var talismanItem = item as TalismanItem;
        if (talismanItem != null)
        {
            mainIcon.sprite = DataBaseController.Instance.TalismanIcon(talismanItem.TalismanType);
            var element = DataBaseController.GetItem<ParameterElement>(Prefab);
            element.Init(ParamType.PPower, talismanItem.power);
            element.Init(ParamType.MDef, talismanItem.costShoot);
            element.transform.SetParent(layout);
            NameLabel.text = talismanItem.name;
        }
        var bonusItem = item as BonusItem;
        if (bonusItem != null)
        {
            var element = DataBaseController.GetItem<ParameterElement>(Prefab);
            element.Init(ParamType.PPower, bonusItem.power);
            element.transform.SetParent(layout);
            NameLabel.text = "name (" + bonusItem.remainUsetime + ")";
        }
        var execItem = item as ExecutableItem;
        if (execItem != null)
        {

            NameLabel.text = execItem.ExecutableType.ToString();
        }
        InitCost(0, item.cost / 3);
        OnInitCallback(item,ItemOwner.Player);
    }

    public void Init(IShopExecute item)
    {
        Clear();
        SpecIcon.gameObject.SetActive(false);
        NameLabel.text = "Level:" + item.Parameter;
        mainIcon.sprite = item.icon;
        InitCost(item.CrystalCost, item.MoneyCost);
        OnInitCallback(null,ItemOwner.Shop);
    }

    private void InitCost(int crystals,int money)
    {
        Debug.Log("Init cost : " + crystals + "    " + money);
        if (crystals > 0)
        {
            var element = DataBaseController.GetItem<ParameterElement>(Prefab);
            element.Init(ItemId.crystal, crystals);
            element.transform.SetParent(moneyLayout);
        }
        if (money > 0)
        {
            var element = DataBaseController.GetItem<ParameterElement>(Prefab);
            element.Init(ItemId.money, money);
            element.transform.SetParent(moneyLayout);
        }
    }

    private void Clear()
    {

        foreach (Transform t in layout)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in moneyLayout)
        {
            Destroy(t.gameObject);
        }
    }
}

