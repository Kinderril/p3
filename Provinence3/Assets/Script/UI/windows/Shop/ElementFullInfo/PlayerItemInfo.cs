using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class PlayerItemInfo : BaseItemInfo
{
    public Transform layout;
    public Image SpecIcon;
    public Text enchantField;
    public void Init(PlayerItem playerItem)
    {
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
            var element = DataBaseController.GetItem<ParameterElement>(Prefab);
            element.Init(p.Key, count);
            element.transform.SetParent(layout);
        }
        var haveSpec = playerItem.specialAbilities != SpecialAbility.none;
        SpecIcon.gameObject.SetActive(haveSpec);
        if (haveSpec)
        {
            SpecIcon.gameObject.SetActive(true);
            SpecIcon.sprite = DataBaseController.Instance.SpecialAbilityIcon(playerItem.specialAbilities);
        }
        mainIcon.sprite = playerItem.IconSprite;
        // PlayerItem.FIRSTCHAR Resources.Load<Sprite>("sprites/PlayerItems/" + playerItem.icon);
        NameLabel.text = playerItem.name;
    }
}

