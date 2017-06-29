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
    public GameObject IsReady;
    public Text Type;

    public void SetProgress(int cur, int trg)
    {
        ProgressField.text = cur + "/" + trg;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
//        ReadyGameObject.gameObject.SetActive(false);
    }

    public void Hide()
    {

        gameObject.SetActive(false);
//        ReadyGameObject.gameObject.SetActive(false);
    }
}
