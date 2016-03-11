using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CraftItemElement : MonoBehaviour
{
    private ExecCraftItem craftItem;
    public GameObject isEnoughtGameobject;
    public Text countField;
    private bool isEnought = false;

    public bool IsEnought
    {
        get { return isEnought; }
    }

    public void Init(ExecCraftItem craftItem)
    {
        this.craftItem = craftItem;
        var myItem = MainController.Instance.PlayerData.GetAllItems()
            .FirstOrDefault(x => (x is ExecCraftItem) && ((ExecCraftItem) x).ItemType == craftItem.ItemType) as ExecCraftItem;
        int myCount = 0;
        if (myItem != null)
        {
            myCount = myItem.count;
        }
        isEnought = myCount >= craftItem.count;
        isEnoughtGameobject.SetActive(isEnought);
        countField.text = Mathf.Clamp(myCount, 0, craftItem.count) + "/" + craftItem.count;

    }

    public void Uprise()
    {

    }
}

