using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CurQuestInfo : MonoBehaviour
{
//    public Text FieldType;
    public Text FieldProgress;
//    public Text FieldDifficulty;
    public GameObject Completed;
    public GameObject NotCompleted;

    public void Init(QuestGiver giver)
    {
        var isCompleted = giver.Status == QuestStatus.ready;
        Completed.gameObject.SetActive(isCompleted);
        NotCompleted.gameObject.SetActive(!isCompleted);
        var name1 = DataBaseController.Instance.GetName(giver.Logic.LogicType);
        FieldProgress.text = name1 + "    " + giver.Difficulty.ToString() + "  " 
            + giver.Logic.CurrentCount + "/" + giver.Logic.TargetCount;
    }

    public void Init(QuestLogicBase giver)
    {
        var isCompleted = true;
        Completed.gameObject.SetActive(isCompleted);
        NotCompleted.gameObject.SetActive(!isCompleted);
        var name1 =DataBaseController.Instance.GetName(giver.LogicType);
        FieldProgress.text = name1 + "    " + giver.Difficulty.ToString() + "  " 
            + giver.CurrentCount + "/" + giver.TargetCount;
    }
}

