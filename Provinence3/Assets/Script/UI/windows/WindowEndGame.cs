using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WindowEndGame : BaseWindow
{
    public Text moneyField;
    public Text crystalField;
    public Text captureField;
    public Text killsField;
    public GameObject goodPicture;
    public GameObject badPicture;
    public Transform craftsLayout;
    public Transform bigItemLayout;
    public BaseSimpleElement PrefabItem;
    public BaseSimpleElement PrefabCraft;

    public override void Init()
    {
        base.Init();
        ClearTransform(craftsLayout);
        ClearTransform(bigItemLayout);
        var level = MainController.Instance.level;
        bool isGoodEnd = level.IsGoodEnd != EndlevelType.bad;
        var collectedMoney = level.GetAllCollectedMoney();
//        crystalField.gameObject.SetActive(false);
        crystalField.transform.parent.gameObject.SetActive(false);
        foreach (var item in collectedMoney)
        {
            Text t = null;
            switch (item.Key)
            {
                case ItemId.money:
                    t = moneyField;
                    break;
                case ItemId.crystal:
                    var haveCrystals = item.Value > 0;
                    crystalField.transform.parent.gameObject.SetActive(haveCrystals);
                    if (haveCrystals)
                    {
                        t = crystalField;
                    }
                    break;
            }
            if (t != null)
                t.text = "+" + item.Value;
        }
        var crafts = level.CollectedCrafts;
        foreach (var craft in crafts)
        {
            ExecCraftItem craftItem = new ExecCraftItem(craft.Key,craft.Value);
            var playerItem = DataBaseController.GetItem<BaseSimpleElement>(PrefabCraft);
            playerItem.Init(craftItem);
            playerItem.gameObject.transform.SetParent(craftsLayout, false); 
        }

        var items = level.CollectedItems;
        foreach (var baseItem in items)
        {
            var playerItem = DataBaseController.GetItem<BaseSimpleElement>(PrefabItem);
            playerItem.Init(baseItem);
            playerItem.gameObject.transform.SetParent(bigItemLayout, false);
        }

        string capt = isGoodEnd ? "Good end" : "Bad ending lose half of gold";
        goodPicture.SetActive(isGoodEnd);
        badPicture.SetActive(!isGoodEnd);
        captureField.text = capt;
        killsField.text =  "Monsters killed:" + level.EnemiesKills.ToString("0");
    }
}

