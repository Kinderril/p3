using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TalismanButton : MonoBehaviour
{
    private SpellItem spellItem;
    private SpellInGame talicLogic;
//    public Slider sliderReady;
    public Image RadialImage;
    public Button button;
    public Image icon;
    public Text chargesField;

    public Image iconTrigger;
    public Text triggerCharges;

    public void Init(SpellItem spell, int countTalismans,Level level,Unit owner)
    {
        this.spellItem = spell;
        talicLogic = SpellInGame.Creat(spellItem, countTalismans, level, owner);
        talicLogic.OnReady += OnReady;
        RadialImage.type = Image.Type.Filled;
        if (spell.SpellData.BaseTrigger != null)
        {
            iconTrigger.sprite = DataBaseController.Instance.SpellTrigger(spell.SpellData.BaseTrigger.TriggerType);
            talicLogic.OnTriggerChargesChange += OnTriggerChargesChange;
            iconTrigger.gameObject.SetActive(true);
        }
        else
        {
            iconTrigger.gameObject.SetActive(false);
        }
        triggerCharges.gameObject.SetActive(false);


        gameObject.SetActive(true);
        icon.sprite = spell.IconSprite;
        OnReady(false, 0,0);
    }

    private void OnTriggerChargesChange(int obj)
    {
        triggerCharges.gameObject.SetActive(obj > 0);
        triggerCharges.text = obj.ToString();
    }

    private void OnReady(bool isReady, float percent,int curCharges)
    {
        RadialImage.gameObject.SetActive(talicLogic.sourseItem.SpellData.Charges != curCharges);
//        sliderReady.value = percent;
        RadialImage.fillAmount = 1 - percent;
        button.interactable = isReady;
        chargesField.text = curCharges.ToString("0");
    }
    
    public void OnClick()
    {
        talicLogic.UseIfCan();
    }

    void OnDestroy()
    {
        if (talicLogic != null)
        {
            talicLogic.OnTriggerChargesChange -= OnTriggerChargesChange;
            talicLogic.Dispose();
            talicLogic.OnReady -= OnReady;
        }
    }
}

