using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WindowSpellMerge : BaseWindow
{
    public RectTransform LayoutSpells;
    public Image Icon1;
    public Image Icon2;

    public SpellMergeElement ElementPrefab;
    public AfterMergeSpellShow SpellShow;
    private List<SpellMergeElement> allElements = new List<SpellMergeElement>(); 

    private SpellItem selectedSpell1;
    private SpellItem selectedSpell2;

    private SpellItem lastSelectedSpell;
    public Image CanMerge;

    public SpellItem SelectedSpell1
    {
        set
        {
            selectedSpell1 = value;
            SetIcon(selectedSpell1,Icon1);
        }
    }
    public SpellItem SelectedSpell2
    {
        set
        {
            selectedSpell2 = value;
            SetIcon(selectedSpell2, Icon2);
        }
    }

    public override void Init()
    {
        CheckCanMerge();
        SpellShow.gameObject.SetActive(false);
        Refresh();
        base.Init();
    }

    private void CheckCanMerge()
    {
        CanMerge.enabled = (selectedSpell1 != null && selectedSpell2 != null);
    }

    public void OnClickElement1()
    {
        SelectedSpell1 = null;
    }

    public void OnClickElement2()
    {
        SelectedSpell2 = null;
    }

    private void Refresh()
    {
        SelectedSpell1 = null;
        SelectedSpell2 = null;
        lastSelectedSpell = null;
        ClearTransform(LayoutSpells);
        allElements.Clear();
        var allSpells = MainController.Instance.PlayerData.GetAllItems().Where(x => x is SpellItem);
        foreach (var allSpell in allSpells)
        {
            var item = DataBaseController.GetItem<SpellMergeElement>(ElementPrefab);
            item.Init(allSpell as SpellItem, OnClickCallback);
            item.transform.SetParent(LayoutSpells, false);
            allElements.Add(item);
        }
    }

    private void CheckSelected()
    {
        foreach (var element in allElements)
        {
            if ((selectedSpell1 != null && element.spellItem == selectedSpell1.BaseItem)
                || (selectedSpell2 != null && element.spellItem == selectedSpell2.BaseItem ))
            {
                element.Select(true);
                continue;
            }
            else
            {
                element.Select(false);
            }
        }
        CheckCanMerge();
    }

    public void OnTryMergeClick()
    {
        if (selectedSpell1 == null || selectedSpell2 == null)
        {
            WindowManager.Instance.InfoWindow.Init(null,"Select 2 spells to merge");
            return;
        }
        WindowManager.Instance.ConfirmWindow.Init(() =>
        {
            var playerData = MainController.Instance.PlayerData;
            playerData.RemoveItem(selectedSpell1);
            playerData.RemoveItem(selectedSpell2);
            var nSpell = SpellMerger.Merge(selectedSpell1.SpellData, selectedSpell2.SpellData);
            SpellsDataBase.SaveSpell(nSpell);
            var nItem = new SpellItem(nSpell);
            playerData.AddItem(nItem);
            SpellShow.Init(nItem, () =>
            {
                Refresh();
            });
        },null,"Do you want to merge spell. You will lose selected spells and get result sepll");
    }

    private void SetIcon(SpellItem spellItem, Image icon)
    {
        lastSelectedSpell = spellItem;
        if (spellItem != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = spellItem.IconSprite;
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
        CheckSelected();
    }

    private void OnClickCallback(SpellMergeElement spellElement)
    {
        var obj = spellElement.spellItem;
        if (selectedSpell1 == obj)
        {
            SelectedSpell1 = null;
            return;
        }
        if (selectedSpell2 == obj)
        {
            SelectedSpell2 = null;
            return;
        }

        if (selectedSpell1 == null)
        {
            SelectedSpell1 = obj;
            return;       
        }
        if (selectedSpell2 == null)
        {
            SelectedSpell2 = obj;
            return;
        }
        if (lastSelectedSpell == selectedSpell1)
        {
            SelectedSpell2 = obj;
            return;
        }
        SelectedSpell1 = obj;
    }
}

