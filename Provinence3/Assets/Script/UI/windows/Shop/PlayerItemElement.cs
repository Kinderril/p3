using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UnderUi
{
    none,
    delete,
    equip
}

public class PlayerItemElement : MonoBehaviour
{
    public Image rareImage;
    public Image iconImage;
    public Image SlotLabel;
    public Image equpedImage;
    public BaseItem PlayerItem;
    public Text enchantField;
    public Text NameField;
    private Transform oldTransforml;
    private Action<PlayerItemElement> callback;
    private bool isDrag = false;
        
    public void Init(BaseItem item,Action<PlayerItemElement> OnClicked)
    {
        this.callback = OnClicked;
        PlayerItem = item;
        Refresh();

    }
    public void Refresh()
    {
        enchantField.gameObject.SetActive(false);
        NameField.text = PlayerItem.Name;
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
        equpedImage.gameObject.SetActive(PlayerItem.IsEquped);
        iconImage.sprite = PlayerItem.IconSprite;
        SlotLabel.sprite = DataBaseController.Instance.SlotIcon(PlayerItem.Slot);
    }

    public void OnPointerClick()
    {
        callback(this);
    }
    
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        oldTransforml = transform.parent;
        transform.SetParent(transform.parent.parent.parent);
        isDrag = true;
    } 
    
    public void Equip(bool val)
    {
        equpedImage.gameObject.SetActive(val);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag)
            transform.position = eventData.position;
    }

}

