using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class BonusItemView : MonoBehaviour
{
    public Image mainIcon;

    public void Init(BonusItem bonusItem)
    {
        mainIcon.sprite = bonusItem.IconSprite;
    }
}

