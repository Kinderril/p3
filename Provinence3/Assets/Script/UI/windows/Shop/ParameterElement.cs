using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ParameterElement : MonoBehaviour
{
    public Text label;
    public Image Icon;

    public void Init(ParamType param,float val)
    {
        Icon.sprite = DataBaseController.Instance.ParameterIcon(param);
        label.text = val.ToString("0");
    }

    public void Init(ItemId param, float val)
    {
        Icon.sprite = DataBaseController.Instance.ItemIcon(param);
        label.text = val.ToString("0");
    }
}

