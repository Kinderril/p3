using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CraftInfoPlace : MonoBehaviour
{
    public Text MainParameterField;
    public Image MainParameterIcon;
    public Transform LayoutSpecials;

    public GameObject PrefabSpecialIcon;
    public GameObject PrefabSecondaryParam;

    public void Init(RecipeItem recipeItem,CatalysItemType? type)
    {
        Utils.ClearTransform(LayoutSpecials);
        var totalPoints = Formuls.GetPlayerItemPointsByLvl(recipeItem.Level) * Formuls.GetSlotCoef(recipeItem.recipeSlot);
        switch (recipeItem.recipeSlot)
        {
            case Slot.physical_weapon:
            case Slot.magic_weapon:
                if (type.HasValue)
                {
                    var spedAbilities = RecipeItem.PosibleAbilities(type.Value);
                    foreach (var specialAbility in spedAbilities)
                    {
                        var icon = DataBaseController.Instance.SpecialAbilityIcon(specialAbility);
                        var img = Instantiate(PrefabSpecialIcon).GetComponent<Image>();
                        img.sprite = icon;
                        img.transform.SetParent(LayoutSpecials,false);
                    }
                }
                var min = totalPoints * 0.5f;
                var max = totalPoints;
                MainParameterField.text = min + " - " + max;
                MainParameterIcon.sprite = DataBaseController.Instance.ParameterIcon(ParamType.PPower);
                
                break;
            case Slot.body:
            case Slot.helm:


                var secondaryParam = HeroShopRandomItem.GetSecondaryParam(totalPoints, recipeItem.recipeSlot);


                break;
            case Slot.Talisman:
                break;
        }
    }
}

