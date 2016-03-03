using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class BaseSimpleElement : MonoBehaviour
{
    public Image rareImage;
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
        if (PlayerItem is PlayerItem)
        {
            var pItem = PlayerItem as PlayerItem;
            rareImage.gameObject.SetActive(pItem.isRare);
            bool haveEnchant = pItem.enchant > 0;
            if (haveEnchant)
            {
                enchantField.text = "+" + pItem.enchant;
            }
            enchantField.gameObject.SetActive(haveEnchant);
        }
        else
        {
            rareImage.gameObject.SetActive(false);
        }

        var exec = PlayerItem as ExecutableItem;
        if (exec != null)
        {
            CountField.gameObject.SetActive(true);
            CountField.text = exec.count.ToString("0");
        }
//        var craft = PlayerItem as RecipeItem;
//        if (craft != null)
//        {
//            
//        }
        
        iconImage.sprite = PlayerItem.IconSprite;
        
        SlotLabel.sprite = DataBaseController.Instance.SlotIcon(PlayerItem.Slot);
    }
}

