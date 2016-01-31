using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class WindowInGame : BaseWindow
{

    public Slider TImeSlider;
    public Slider HealthSlider;
    public Text moneyField;
    public WeaponChooserView WeaponChooser;
    public UIMain UiControls;
    public Transform hitTransform;
    public List<TalismanButton> TalismanButtons;
    public Transform moneyContainer;

    public override void Init()
    {
        base.Init();
        UiControls.Init();
        MainController.Instance.level.OnLeft += OnLeft;
        MainController.Instance.level.OnItemCollected += OnItemCollected;
        MainController.Instance.level.MainHero.OnGetHit += OnHeroHit;
        MainController.Instance.level.MainHero.OnWeaponChanged += OnWeaponChanged;
        WeaponChooser.Init();
        UiControls.Enable(true);
        int index = 0;
        var allTalismans = MainController.Instance.PlayerData.GetAllWearedItems().Where(x => x.Slot == Slot.Talisman);
        foreach (var talic in allTalismans)
        {
            var talismain = talic as TalismanItem;
            TalismanButtons[index].Init(talismain, allTalismans.Count());
            index++;
        }
        for (int i = index; i < TalismanButtons.Count; i++)
        {
            TalismanButtons[i].gameObject.SetActive(false);
        }
        HealthSlider.value = 1;
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
        UiControls.Enable(false);
        MainController.Instance.level.OnLeft -= OnLeft;
        MainController.Instance.level.OnItemCollected -= OnItemCollected;
        MainController.Instance.level.MainHero.OnGetHit -= OnHeroHit;
        MainController.Instance.level.MainHero.OnWeaponChanged -= OnWeaponChanged;
    }

    private void OnItemCollected(ItemId arg1, float arg2,float delta)
    {
        FlyingNumbers item;
        switch (arg1)
        {
            case ItemId.money:
                moneyField.text = arg2.ToString("00");
                item = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberInUI);
                //item = DataBaseController.GetItem(DataBaseController.Instance.FlyingNumber, moneyField.transform.position);
                item.transform.SetParent(moneyContainer);
//                item.transform.position = moneyField.transform.position;
                item.Init(GetDeltaStr(delta) + " Gold", DataBaseController.Instance.GetColor(arg1),FlyNumerDirection.non,26);
                break;
            case ItemId.crystal:
                MainController.Instance.level.MessageAppear("You found crystal", Color.green, DataBaseController.Instance.ItemIcon(ItemId.crystal));
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

    private void OnHeroHit(float arg1, float arg2,float delta)
    {
        if (delta > 1)
        {
            //Debug.Log("OnHeroHit " + arg1 + "/" + arg2  + " d:" + delta);
            var item = DataBaseController.Instance.Pool.GetItemFromPool<FlyingNumbers>(PoolType.flyNumberInUI);
            //var item = DataBaseController.GetItem(DataBaseController.Instance.FlyingNumber, hitTransform.position);
            item.transform.SetParent(transform);
            item.transform.position = hitTransform.position;
            Color color = DataBaseController.Instance.GetColor(ItemId.health);
            bool isPlus = (delta > 0);
            item.Init(GetDeltaStr(delta), color);
        }
        HealthSlider.value = arg1 / arg2;
    }

    private void OnLeft(float arg1, float arg2)
    {
        var v = 1f - arg1/arg2;
        TImeSlider.value = v;
    }

}
