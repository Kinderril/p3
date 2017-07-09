using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class AfterMergeSpellShow : MonoBehaviour
{

    public SpellItemInfo SpellItemInfo;
    private Action onClose;

    public void Init(SpellItem item, Action onClose)
    {
        this.onClose = onClose;
        gameObject.SetActive(true);
        SpellItemInfo.Init(item,false);
    }

    public void OnCloseClick()
    {
        gameObject.SetActive(false);
        onClose();
    }
}

