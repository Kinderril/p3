using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeWindow : MonoBehaviour
{
    public Image ItemImage;
    public Image EnchantImage;
    private IEnhcant item;
    public Button safeButton;
    private Action<IEnhcant> onClose;

    public static void Creat()
    {
//        WindowManager.Instance.
    }

    public void Init(IEnhcant item,Action<IEnhcant> onClose)
    {
        this.item = item;
        this.onClose = onClose;
        var canSafe = MainController.Instance.PlayerData.CanPay(ItemId.crystal, PlayerData.CRYSTAL_SAFETY_ENCHANT);
        safeButton.interactable = canSafe;
        gameObject.SetActive(true);
        ItemImage.sprite = item.BaseItem.IconSprite;
        var exec = MainController.Instance.PlayerData.CanBeUpgraded(item);
        EnchantImage.sprite = exec.IconSprite;
    }

    public void OnClose()
    {
        onClose(item);
        gameObject.SetActive(false);
    }

    public void OnSimpleEnchant()
    {

        WindowManager.Instance.ConfirmWindow.Init(
            () =>
            {
                MainController.Instance.PlayerData.TryToEnchant(item, false);
                OnClose();
            },
            OnClose, 
            "You want to do enchant?");
    }

    public void OnSafeEnchant()
    {
        WindowManager.Instance.ConfirmWindow.Init(() =>
        {
            MainController.Instance.PlayerData.TryToEnchant(item, true);
            OnClose();
        },
            OnClose, "You want to do safe enchant?");
    }
}

