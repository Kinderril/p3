using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class BaseSimpleElement : MonoBehaviour
{
    public RarityElement rareImage;
    public Image iconImage;
    public Image SlotLabel;
    public Text enchantField;
    public Text NameField;
    public Text CountField;
    public BaseItem PlayerItem;

    public virtual void Init(BaseItem item)
    {
        PlayerItem = item;
        Refresh();

    }
    public virtual void Refresh()
    {
        enchantField.gameObject.SetActive(false);
        NameField.text = PlayerItem.Name;
        CountField.gameObject.SetActive(false);
        iconImage.sprite = PlayerItem.IconSprite;
        if (PlayerItem is PlayerItem)
        {
            var pItem = PlayerItem as PlayerItem;
            NameField.color = DataBaseController.Instance.GetColor(pItem.Rare);
            rareImage.Set(pItem.Rare);
            bool haveEnchant = pItem.enchant > 0;
            if (haveEnchant)
            {
                enchantField.text = "+" + pItem.enchant;
            }
            enchantField.gameObject.SetActive(haveEnchant);
        }
        else
        {
            NameField.color = DataBaseController.Instance.GetColor(Rarity.Normal);
            rareImage.gameObject.SetActive(false);
        }

        var exec = PlayerItem as ExecutableItem;
        if (exec != null)
        {
            CountField.gameObject.SetActive(true);
            CountField.text = exec.count.ToString("0");
        }

        
        SlotLabel.sprite = DataBaseController.Instance.SlotIcon(PlayerItem.Slot);
        if (SlotLabel.sprite == null)
        {
            SlotLabel.gameObject.SetActive(false);
        }
    }
}

