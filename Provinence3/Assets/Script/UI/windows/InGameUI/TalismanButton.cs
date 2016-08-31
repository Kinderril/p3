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
//    public Slider sliderReady;
    public Image RadialImage;
    public Button button;
    public Image icon;
    public Text chargesField;

    public void Init(TalismanItem talic, int countTalismans,Level level)
    {
        this.TalismanItem = talic;
        talicLogic = Talisman.Creat(TalismanItem, countTalismans, level);
        talicLogic.OnReady += OnReady;
        RadialImage.type = Image.Type.Filled;

        gameObject.SetActive(true);
        icon.sprite = talic.IconSprite;
        OnReady(false, 0,0);
    }

    private void OnReady(bool isReady, float percent,int curCharges)
    {
        RadialImage.gameObject.SetActive(talicLogic.sourseItem.MaxCharges != curCharges);
//        sliderReady.value = percent;
        RadialImage.fillAmount = 1 - percent;
        button.interactable = isReady;
        chargesField.text = curCharges.ToString("0");
    }
    
    public void OnClick()
    {
        talicLogic.UseIfCan();
    }

    void OnDestroy()
    {
        if (talicLogic != null)
        {
            talicLogic.Dispose();
            talicLogic.OnReady -= OnReady;
        }
    }
}

