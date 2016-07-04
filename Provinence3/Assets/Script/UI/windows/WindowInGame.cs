﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class WindowInGame : BaseWindow
{

    public Slider TImeSlider;
    public Slider HealthSlider;
    public MonsterInfo MonsterInfo;
    public Text moneyField;
    public WeaponChooserView WeaponChooser;
    public UIMain UiControls;
    public Transform hitTransform;
    public List<TalismanButton> TalismanButtons;
    public Transform moneyContainer;
    public Transform itemsContainer;
    public PreStartWindow PreStartWindow;
    private Level level;

    public override void Init<T>(T obj)
    {
        WindowManager.Instance.MainBack.gameObject.SetActive(false);
        base.Init(obj);
        level = obj as Level;
        moneyField.text = 0.ToString("0");
        UiControls.Init(level);
        level.OnLeft += OnLeft;
        level.OnItemCollected += OnItemCollected;
        level.OnCraftItemCollected += OnCraftItemCollected;
        level.MainHero.OnGetHit += OnHeroHit;
        level.MainHero.OnWeaponChanged += OnWeaponChanged;
        WeaponChooser.Init(level);
        int index = 0;
        var allTalismans = MainController.Instance.PlayerData.GetAllWearedItems().Where(x => x.Slot == Slot.Talisman);
        foreach (var talic in allTalismans)
        {
            var talismain = talic as TalismanItem;
            TalismanButtons[index].Init(talismain, allTalismans.Count(),level);
            index++;
        }
        for (int i = index; i < TalismanButtons.Count; i++)
        {
            TalismanButtons[i].gameObject.SetActive(false);
        }
        HealthSlider.value = 1;
        ShowPreStartWindow();
        MonsterInfo.Init();
    }


    private void ShowPreStartWindow()
    {
        PreStartWindow.Init(level,() =>
        {
            level.Start();
            UiControls.Enable(true);
        });
    }

    private void OnCraftItemCollected(CraftItemType arg1, int delta)
    {
        FlyingNumbers item;
        item = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberInUI);
        item.transform.SetParent(itemsContainer);
        item.Init(GetDeltaStr(delta) + " " + arg1.ToString(), DataBaseController.Instance.GetColor(arg1), FlyNumerDirection.non, 36);

    }

    private void OnWeaponChanged(Weapon obj)
    {
        WeaponChooser.SetWeapon(obj);
    }

    public void EndGame()
    {
        UiControls.Enable(false);
    }

    public override void Close()
    {
        base.Close();
        WindowManager.Instance.MainBack.gameObject.SetActive(true);
        MonsterInfo.DeInit();
        UiControls.Enable(false);
        level.OnLeft -= OnLeft;
        level.OnItemCollected -= OnItemCollected;
        level.MainHero.OnGetHit -= OnHeroHit;
        level.MainHero.OnWeaponChanged -= OnWeaponChanged;
        level.OnCraftItemCollected -= OnCraftItemCollected;
    }

    private void OnItemCollected(ItemId arg1, float arg2,float delta)
    {
        FlyingNumbers item;
        switch (arg1)
        {
            case ItemId.money:
                moneyField.text = arg2.ToString("00");
                item = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberInUI);
                item.transform.SetParent(moneyContainer);
                item.Init(GetDeltaStr(delta) + " Gold", DataBaseController.Instance.GetColor(arg1),FlyNumerDirection.non,26);
                break;
            case ItemId.crystal:
                level.MessageAppear("You found crystal", Color.green, DataBaseController.Instance.ItemIcon(ItemId.crystal));
                break;
            case ItemId.energy:
                item = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberInUI);
                item.transform.SetParent(moneyContainer);
                item.Init("+" + Mathf.Abs(delta).ToString("0")+ " Energy", DataBaseController.Instance.GetColor(arg1), FlyNumerDirection.non,30);
                break;
        }
    }

    private string GetDeltaStr(float delta)
    {
        return ((delta > 0) ? "+" : "") + delta.ToString("0");
    }

    private void OnHeroHit(float cur_HP, float maxHp,float delta)
    {
        FlyingNumbers number = null;
        Color color = Color.green;
        if (delta >= 1)
        {
            number = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberInUI);
//            color =Color.green;
        }
        else if (delta <=-1)
        {
            number = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberInUI);
            color =  DataBaseController.Instance.GetColor(ItemId.health);
        }
        if (number != null)
        {
            number.transform.SetParent(transform);
            number.transform.position = hitTransform.position;
            number.Init(GetDeltaStr(delta), color);
        }
        HealthSlider.value = cur_HP / maxHp;
    }

    private void OnLeft(float arg1, float arg2)
    {
        var v = 1f - arg1/arg2;
        TImeSlider.value = v;
    }

}
