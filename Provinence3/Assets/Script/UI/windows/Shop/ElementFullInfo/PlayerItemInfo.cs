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
        var paramsAlt = new Dictionary<ParamType,float> ();
//        foreach (var parameter in playerItem.parameters)
//        {
//            paramsAlt.Add(parameter.Key, parameter.Value);
//        }
        List<KeyValuePair<ParamType, float>> myList = playerItem.parameters.ToList();

        myList.Sort(
            delegate (KeyValuePair<ParamType, float> pair1,
            KeyValuePair<ParamType, float> pair2)
            {
                var result = pair1.Value.CompareTo(pair2.Value);
                if (result == 1)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        );
        foreach (var p in myList)
        {
            var count = p.Value;
            if (!enchanted)
            {
                enchanted = true;
                count = count * ( 1 +  playerItem.enchant / PlayerItem.ENCHANT_PLAYER_COEF);
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
            SpecName.text = GetSpecialAbilityInfo(playerItem.specialAbilities);
            SpecIcon.sprite = DataBaseController.Instance.SpecialAbilityIcon(playerItem.specialAbilities);
            SpecIcon.transform.parent.transform.SetAsLastSibling();
        }
        mainIcon.sprite = playerItem.IconSprite;
        NameLabel.text = playerItem.name;
    }

    private string GetSpecialAbilityInfo(SpecialAbility sprcial)
    {
        string ss = sprcial.ToString();
        switch (sprcial)
        {
            case SpecialAbility.critical:
                ss = "Add chance to get critical strike with " + (Formuls.CRIT_CHANCE) + "% chance for " +
                     (Formuls.CRIT_COEF*100) + "% extra damage";
                break;
            case SpecialAbility.push:
                break;
            case SpecialAbility.slow:
                ss = "Each strike slows target for " + (100 - Formuls.SLOW_COEF*100).ToString("0") + "%";
                break;
            case SpecialAbility.removeDefence:
                ss = "Each strike decrease target defence for " + (100 - Formuls.REMOVE_DEF_COEF * 100) + "%";
                break;
            case SpecialAbility.vampire:
                ss = "Every strike heals hero for " + (100 * Formuls.VAMP_COEF) + "% of damage";
                break;
            case SpecialAbility.chain:
                break;
            case SpecialAbility.clear:
                ss = "Bullets ignore " + (100-(Formuls.CLEAR_COEF*100)) + "% of target defence";
                break;
            case SpecialAbility.dot:
                ss = "Do nothing =)";
                break;
            case SpecialAbility.stun:
                ss = "Add chance " + Formuls.CHANCE_STUN + "%  to stun target with for " +
                     (Formuls.STUN_TIME_SEC) + " seconds";
                break;
            case SpecialAbility.distance:
                var min1 = 0;
                var max1 =100 * WeaponParameters.MID_MAX_RANGE * Formuls.DISTANCE_COEF;
                ss = "Add extra damage, dependce on distance to target "
                    + min1.ToString("0") + "% on zero range to: "
                    + max1.ToString("0") + "% on max range";
                break;
            case SpecialAbility.hp:
                var min = 100 * (1f - 0f/11f)/Formuls.HP_SKILL_COEF;
                var max = 100 * (1f - 11f/11f)/Formuls.HP_SKILL_COEF;
                ss = "Add extra damage dependce on current health of hero from " 
                    + min.ToString("0") + "% on zero hp to: "
                    + max.ToString("0") + "% on max hp";
                break;
            case SpecialAbility.shield:
                ss = "Add shield to hero for each hit by current difficulty level" ;
                break;
        }
        return ss;
    }
}

