using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class QuestInfo : MonoBehaviour
{
    public Text ProgressField;
    public GameObject ReadyGameObject;

    public void SetProgress(int cur, int trg)
    {
        ProgressField.text = cur + "/" + trg;
    }
}
