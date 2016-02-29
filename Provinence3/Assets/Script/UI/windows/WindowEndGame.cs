using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WindowEndGame : BaseWindow
{
    public Text moneyField;
    public GameObject crystalField;
    public Text captureField;
    public Text killsField;
    public GameObject goodPicture;
    public GameObject badPicture;
    public Transform craftsLayout;
    public Transform bigItemLayout;
    public TurnerPlayerItemElement TurnerPlayerItemElement;
    public PlayerItemElement PrefabPlayerItemElement;

    public override void Init()
    {
        base.Init();
        ClearTransform(craftsLayout);
        ClearTransform(bigItemLayout);
        var level = MainController.Instance.level;
        bool isGoodEnd = level.IsGoodEnd != EndlevelType.bad;
        var collectedMoney = level.GetAllCollectedMoney();
        crystalField.SetActive(false);
        foreach (var item in collectedMoney)
        {
            Text t = null;
            switch (item.Key)
            {
                case ItemId.money:
                    t = moneyField;
                    break;
                case ItemId.crystal:
                    if (item.Value > 0)
                        crystalField.SetActive(true);
                    break;
            }
            if (t != null)
                t.text = "+" + item.Value;
        }
        var crafts = level.CollectedCrafts;
        foreach (var craft in crafts)
        {
            ExecCraftItem craftItem = new ExecCraftItem(craft.Key,craft.Value);
            var playerItem = DataBaseController.GetItem<PlayerItemElement>(PrefabPlayerItemElement);
            playerItem.Init(craftItem, element => { });
            playerItem.gameObject.transform.SetParent(craftsLayout);
        }

        var items = level.CollectedItems;
        foreach (var baseItem in items)
        {
            var playerItem = DataBaseController.GetItem<TurnerPlayerItemElement>(TurnerPlayerItemElement);
            playerItem.Init(baseItem, element => { });
            playerItem.gameObject.transform.SetParent(bigItemLayout);
        }

        string capt = isGoodEnd ? "Good end" : "Bad ending lose half of gold";
        goodPicture.SetActive(isGoodEnd);
        badPicture.SetActive(!isGoodEnd);
        captureField.text = capt;
        killsField.text =  "/";

    }

}

