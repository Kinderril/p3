using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class SpellMergeElement : MonoBehaviour
{
    public Image Icon;
    public GameObject SelectedObj;
    private bool selected = false;
    public SpellItem spellItem;
    private Action<SpellMergeElement> OnClickCallback;

    public void Init(SpellItem spellItem,Action<SpellMergeElement> OnClickCallback)
    {
        this.spellItem = spellItem;
        this.OnClickCallback = OnClickCallback;
        Icon.sprite = spellItem.IconSprite;
        SelectedObj.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        OnClickCallback(this);
    }

    public void Select(bool val)
    {
        selected = val;
        SelectedObj.gameObject.SetActive(selected);
    }
}

