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
    public override void Init()
    {
        base.Init();
        var level = MainController.Instance.level;
        bool isGoodEnd = level.IsGoodEnd;
        var items = level.GetAllCollectedItems();
        crystalField.SetActive(false);
        foreach (var item in items)
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
        string capt = isGoodEnd ? "Good end" : "Bad ending lose half of gold";
        goodPicture.SetActive(isGoodEnd);
        badPicture.SetActive(!isGoodEnd);
        captureField.text = capt;
        killsField.text =  "/";

    }

}

