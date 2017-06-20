using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WindowSellWithCount : ConfirmWindow
{
    public Slider Slider;
    public Text FieldCurrentCount;
    private int curVal = 1;
    private Action<int> onConfirm;

    public void Init(Action<int> onConfirm, Action onReject, string ss,int maxCount)
    {
        curVal = 1;
        this.onConfirm = onConfirm;
        base.Init(null, onReject, ss);
        Slider.maxValue = maxCount;
        Slider.minValue = 1;
        OnSliderChange(curVal);
    }

    public void OnSliderChange(float val)
    {
        curVal = (int)Slider.value;
        FieldCurrentCount.text = curVal.ToString();
    }

    public override void OnConfirmClick()
    {
        if (onConfirm != null)
        {
            onConfirm(curVal);
        }
        base.OnConfirmClick();
    }

    public override void OnRejectClick()
    {
        base.OnRejectClick();
    }
}

