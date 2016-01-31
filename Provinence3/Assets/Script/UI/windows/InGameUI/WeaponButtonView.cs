using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class WeaponButtonView : MonoBehaviour
{
    public Image OpenIcon;
    public Image CloseIcon;
    public WeaponType WeaponType;

    public void Select(bool val)
    {
        OpenIcon.gameObject.SetActive(val);
        CloseIcon.gameObject.SetActive(!val);
    }

}

