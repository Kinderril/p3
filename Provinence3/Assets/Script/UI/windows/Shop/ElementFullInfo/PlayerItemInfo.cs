using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class PlayerItemInfo : BaseItemInfo
{
    public Transform layoutParam;
    public Image SpecIcon;
    public Text enchantField;
    public void Init(PlayerItem playerItem)
    {
        base.Init(playerItem);
        bool haveEnchant = playerItem.enchant > 0;
        if (haveEnchant)
        {
            enchantField.text = "+" + playerItem.enchant;
            enchantField.gameObject.SetActive(true);
        }
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
        SpecIcon.gameObject.SetActive(haveSpec);
        if (haveSpec)
        {
            SpecIcon.gameObject.SetActive(true);
            SpecIcon.sprite = DataBaseController.Instance.SpecialAbilityIcon(playerItem.specialAbilities);
        }
        mainIcon.sprite = playerItem.IconSprite;
        NameLabel.text = playerItem.name;
    }
}

