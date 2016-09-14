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
    public Slider BossSpawnSlider;
    public MonsterInfo MonsterInfo;
    public Text moneyField;
    public WeaponChooserView WeaponChooser;
    public UIMain UiControls;
    public Transform hitTransform;
//    private List<TalismanButton> TalismanButtons;
    public TalismanButton PrefabTalismanButton;
    public Transform TalismanButtonsLayout;
    public Transform moneyContainer;
    public Transform itemsContainer;
    public PreStartWindow PreStartWindow;
    public WindowPause WindowPause;
    public QuestInfo QuestActive;
    private Level level;

    public override void Init<T>(T obj)
    {
        WindowManager.Instance.MainBack.gameObject.SetActive(false);
        base.Init(obj);
        ClearTransform(TalismanButtonsLayout);
        level = obj as Level;
        moneyField.text = 0.ToString("0");
        UiControls.Init(level);
        level.Energy.OnLeft += OnLeft;
        level.Energy.OnRage += OnRage;
        level.OnItemCollected += OnItemCollected;
        level.OnCraftItemCollected += OnCraftItemCollected;
        level.MainHero.OnGetHit += OnHeroHit;
        level.MainHero.OnWeaponChanged += OnWeaponChanged;
        Map.Instance.SetCallBackMonstersReady(
        () =>
        {
            Map.Instance.BossSpawner.OnBossGetEnergy += OnBossGetEnergy;
        });
        level.QuestController.OnQuestStatusChanges += OnQuestStatusChanges;
        level.QuestController.OnQuestProgress += OnQuestProgress;
        level.OnPause += OnPause;
        QuestActive.gameObject.SetActive(false);

        WindowPause.gameObject.SetActive(false);
        WeaponChooser.Init(level);
//        int index = 0;
        var allTalismans = MainController.Instance.PlayerData.GetAllWearedItems().Where(x => x.Slot == Slot.Talisman).ToList();
//        TalismanItem healItem = new TalismanItem(Formuls.GetTalismanPointsByLvl(level.difficult),TalismanType.heal);
//        allTalismans.Add(healItem);
        foreach (var talic in allTalismans)
        {
            var talismain = talic as TalismanItem;
            var tBtn = DataBaseController.GetItem<TalismanButton>(PrefabTalismanButton);
            tBtn.transform.SetParent(TalismanButtonsLayout,false);
            //            TalismanButtons.Add(tBtn);

            tBtn.Init(talismain, allTalismans.Count(),level);
//            index++;
        }
//        for (int i = index; i < TalismanButtons.Count; i++)
//        {
//            TalismanButtons[i].gameObject.SetActive(false);
//        }
        BossSpawnSlider.value = 0;
        HealthSlider.value = 1;
        ShowPreStartWindow();
        MonsterInfo.Init();
    }

    private void OnQuestProgress(QuestGiver arg1, int cur, int trg)
    {
        Debug.Log("Quest progress " + cur);
        QuestActive.SetProgress(cur,trg);
    }

    private void OnQuestStatusChanges(QuestGiver obj)
    {
        switch (obj.Status)
        {
            case QuestStatus.started:
                MainController.Instance.level.MessageAppear("Quest Started",obj.Logic.AppearMessage(), Color.white);
                QuestActive.Activate();
                QuestActive.SetProgress(obj.Logic.CurrentCount, obj.Logic.TargetCount);
                break;
            case QuestStatus.ready:
                QuestActive.ReadyGameObject.gameObject.SetActive(true);
                break;
            case QuestStatus.end:
                MainController.Instance.level.MessageAppear("Quest Complete", "", Color.cyan);
                QuestActive.Hide();
                break;
        }
    }

    private void OnPause(bool obj)
    {
        if (obj)
        {
            WindowPause.Init(level);
        }
        else
        {
            WindowPause.Close();
        }
    }

    public void OnPauseClick()
    {
        level.Pause();
    }

    private void OnBossGetEnergy(int arg1, int arg2)
    {
        var energy = (float)arg1 / (float)arg2;
        Debug.Log("Energy:" + energy);
        BossSpawnSlider.value = energy;
    }

    private void OnRage()
    {
        
    }


    private void ShowPreStartWindow()
    {
        PreStartWindow.Init(level,() =>
        {
            level.StartLevel();
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
        level.Energy.OnLeft -= OnLeft;
        level.Energy.OnRage -= OnRage;
        level.OnItemCollected -= OnItemCollected;
        level.MainHero.OnGetHit -= OnHeroHit;
        level.MainHero.OnWeaponChanged -= OnWeaponChanged;
        level.OnCraftItemCollected -= OnCraftItemCollected;
        ClearTransform(TalismanButtonsLayout);
        Map.Instance.BossSpawner.OnBossGetEnergy -= OnBossGetEnergy;
        level.OnPause -= OnPause;
        level.QuestController.OnQuestStatusChanges -= OnQuestStatusChanges;
        level.QuestController.OnQuestProgress -= OnQuestProgress;
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
                level.MessageAppear("You found crystal", "" ,Color.green, DataBaseController.Instance.ItemIcon(ItemId.crystal));
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
