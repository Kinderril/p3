using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class DifficultyChooser : MonoBehaviour
{
    public Text PenaltyText;
    public Text CurDif;
    public Button UpBtn;
    public Button downBtn;
    private int maxLevel;
    private Action<int> OnDifChanges;
    private int curLvl;

    public void Init(Action<int> OnDifChanges)
    {
        this.OnDifChanges = OnDifChanges;
        maxLevel = MainController.Instance.PlayerData.Level;
        curLvl = maxLevel;
        OnChangesUp(true);
    }

    public void OnChangesUp(bool v)
    {
        if (v)
        {
            curLvl++;
        }
        else
        {
            curLvl--;
        }
        curLvl = Mathf.Clamp(curLvl, 1, maxLevel);
        UpBtn.interactable = curLvl != maxLevel;
        downBtn.interactable = curLvl != 1;
        CurDif.text = curLvl.ToString("0");
        var p = Level.GetPenalty(curLvl);
        PenaltyText.gameObject.SetActive(p <1f);
        PenaltyText.text = "Gold Penalty is:" + ((1f-p)*100f).ToString("00") + "%";
        OnDifChanges(curLvl);
    }
}

