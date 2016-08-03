using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WindowPersonage : BaseWindow
{
    public ParameterUpgradeElement PrefabParameterUpgrade;
    private List<ParameterUpgradeElement> elements = new List<ParameterUpgradeElement>();
    public AllParametersContainer AllParametersContainer;
    public Transform layout;
    public Button LevelUpButton;
    public Text moneyField;
    public Text crystalField;
    public Text levelField;
    public Text alocatedField;
    public Text costNextLevelField;

    public override void Init()
    {
        base.Init();
        moneyField.text = MainController.Instance.PlayerData.playerInv[ItemId.money].ToString("0");
        crystalField.text = MainController.Instance.PlayerData.playerInv[ItemId.crystal].ToString("0");
        OnLevelUp(0);
        elements.Clear();
        //elementsParams.Clear();
        LoadParameters();
        AllParametersContainer.Init();
        MainController.Instance.PlayerData.OnParametersChange += OnParametersChange;
        MainController.Instance.PlayerData.OnCurrensyChanges += OnCurrensyChanges;
        MainController.Instance.PlayerData.OnLevelUp += OnLevelUp;
    }

    private void OnLevelUp(int obj)
    {
        LevelUpButton.interactable = MainController.Instance.PlayerData.CanUpgradeLevel();
        var ap = MainController.Instance.PlayerData.AllocatedPoints;
        var lvl = MainController.Instance.PlayerData.Level;
        alocatedField.text = "Remain:"+ ap.ToString("0") + "/" + ((lvl-1)*PlayerData.POINTS_PER_LVL);
        levelField.text = lvl.ToString("0");
//        var cost = DataBaseController.Instance.DataStructs.costParameterByLvl[lvl];
        costNextLevelField.text = Formuls.LevelUpCost(lvl).ToString("0");
        UpgradeAllMainElements();
    }

    private void UpgradeAllMainElements()
    {

        foreach (var parameterUpgradeElement in elements)
        {
            parameterUpgradeElement.UpgradeData();
        }
    }

    private void OnCurrensyChanges(ItemId arg1, int arg2)
    {
        switch (arg1)
        {
            case ItemId.money:
                moneyField.text = arg2.ToString("0");
                break;
            case ItemId.crystal:
                crystalField.text = arg2.ToString("0");
                break;
        }
    }
    private void OnParametersChange(Dictionary<MainParam, int> obj)
    {
        UpgradeAllMainElements();
        AllParametersContainer.UpgradeValues();
        OnLevelUp(0);
    }


    public void OnLevelUpClicked()
    {
        var cost =  Formuls.LevelUpCost(MainController.Instance.PlayerData.Level);
        WindowManager.Instance.ConfirmWindow.Init(() =>
        {
            MainController.Instance.PlayerData.LevelUp();
        }, () =>
        {
            
        },"Do you want to level up for " + cost.ToString("0") + " gold?");
        
    }

    private void LoadParameters()
    {
        var baseParams = MainController.Instance.PlayerData.MainParameters;
        foreach (var baseParam in baseParams)
        {
            var item = DataBaseController.GetItem(PrefabParameterUpgrade);
            item.Init(baseParam.Key);
            item.gameObject.transform.SetParent(layout,false);
            elements.Add(item);
        }
        Utils.Sort(elements,GetPriority);
    }

    private int GetPriority(ParameterUpgradeElement arg1)
    {
        switch (arg1.TypeParam)
        {
            case MainParam.HP:
                return 1;
            case MainParam.DEF:
                return 2;
            case MainParam.ATTACK:
                return 3;
        }
        return 0;
    }


    public override void Close()
    {
        base.Close();
        ClearTransform(layout);
        MainController.Instance.PlayerData.OnParametersChange -= OnParametersChange;
        MainController.Instance.PlayerData.OnCurrensyChanges -= OnCurrensyChanges;
        MainController.Instance.PlayerData.OnLevelUp -= OnLevelUp;
    }
}

