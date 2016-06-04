using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ItemWindow : MonoBehaviour
{
    public ItemInfoElement ItemInfoElement;

    public void OnClickOk()
    {
        gameObject.SetActive(false);
    }

    public void Init(BaseItem item)
    {
        gameObject.SetActive(true);
        ItemInfoElement.Init(item);
    }
}

