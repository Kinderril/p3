using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TalismanButton : MonoBehaviour
{
    private TalismanItem TalismanItem;
    private Talisman talicLogic;
    public Slider sliderReady;
    public Button button;
    public Image icon;

    public void Init(TalismanItem talic, int countTalismans)
    {
        this.TalismanItem = talic;
        talicLogic = Talisman.Creat(TalismanItem, countTalismans);
        talicLogic.OnReady += OnReady;
        gameObject.SetActive(true);
        var spr = DataBaseController.Instance.TalismanIcon(talic.TalismanType);
      //  Debug.Log("Talisman inited " + talic.TalismanType + "   " + icon.gameObject.name);
        icon.sprite = spr;
        OnReady(false, 0);
    }

    private void OnReady(bool isReady, float percent)
    {
        sliderReady.gameObject.SetActive(!isReady);
        sliderReady.value = percent;
        button.interactable = isReady;
    }


    public void OnClick()
    {
      //  Debug.Log("Talisman USE!");
        talicLogic.Use();
    }

    void OnDestroy()
    {
        talicLogic.OnReady -= OnReady;
    }
}

