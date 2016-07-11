using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class PlayerItemInfo : UpgWearingInvItemInfo
{
    public Transform layoutParam;
    public Image SpecIcon;
    public Text SpecName;
    
    public void Init(PlayerItem playerItem,bool WithButtons)
    {
        base.Init(playerItem,true, WithButtons);
        SetEnchant(playerItem.enchant, playerItem.enchant > 0);
        bool enchanted = false;
        foreach (var p in playerItem.parameters)
        {
            var count = p.Value;
            if (!enchanted)
            {
                enchanted = true;
                count += count * playerItem.enchant / 5;
            }
            var element = DataBaseController.GetItem<ParameterElement>(DataBaseController.Instance.DataStructs.PrefabsStruct.ParameterElement);
            element.Init(p.Key, count);
            element.transform.SetParent(layoutParam,false);
        }
        var haveSpec = playerItem.specialAbilities != SpecialAbility.none;
        SpecIcon.transform.parent.gameObject.SetActive(haveSpec);
        if (haveSpec)
        {
            SpecIcon.gameObject.SetActive(true);
            SpecName.text = playerItem.specialAbilities.ToString();
            SpecIcon.sprite = DataBaseController.Instance.SpecialAbilityIcon(playerItem.specialAbilities);
            SpecIcon.transform.parent.transform.SetAsLastSibling();
        }
        mainIcon.sprite = playerItem.IconSprite;
        NameLabel.text = playerItem.name;
    }
}

