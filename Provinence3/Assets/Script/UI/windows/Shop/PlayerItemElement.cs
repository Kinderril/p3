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
    private Transform oldTransforml;
    private Action<PlayerItemElement> callback;
    private float startTakeTime = 0;
    private bool isDrag = false;
        
    public void Init(BaseItem item,Action<PlayerItemElement> OnClicked)
    {
        PlayerItem = item;
        this.callback = OnClicked;
        if (item is PlayerItem)
        {
            rareImage.gameObject.SetActive((item as PlayerItem).isRare);
        }
        equpedImage.gameObject.SetActive(item.IsEquped);
        iconImage.sprite = item.IconSprite;
        SlotLabel.sprite = DataBaseController.Instance.SlotIcon(item.Slot);
    }

    public void OnPointerClick()
    {
        callback(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startTakeTime = Time.time;
        /*
        if (!isDrag)
        {
            StartCoroutine(Wait());
            callback(this);
        }*/
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        oldTransforml = transform.parent;
        transform.SetParent(transform.parent.parent.parent);
        isDrag = true;
    } 

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
        //var deltaTime = Time.time - startTakeTime; 
        /*
        var res = IsOnWhat(eventData.position);
        transform.SetParent(oldTransforml);
        switch (res)
        {
            case UnderUi.delete:
                MainController.Instance.PlayerData.Sell(PlayerItem);
                break;
            case UnderUi.equip:
                MainController.Instance.PlayerData.EquipItem(PlayerItem);
                break;
        }*/
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

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
    }
}

