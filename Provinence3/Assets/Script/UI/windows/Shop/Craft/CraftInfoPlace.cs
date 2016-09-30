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
                MainParameterField.text = min.ToString("0") + " - " + max;
                switch (recipeItem.Slot)
                {
                    case Slot.physical_weapon:
                        MainParameterIcon.sprite = DataBaseController.Instance.ParameterIcon(ParamType.PPower);
                        break;
                    case Slot.magic_weapon:
                        MainParameterIcon.sprite = DataBaseController.Instance.ParameterIcon(ParamType.MPower);
                        break;
                }
                
                break;
            case Slot.body:
            case Slot.helm:

                switch (recipeItem.Slot)
                {
                    case Slot.body:
                        MainParameterIcon.sprite = DataBaseController.Instance.ParameterIcon(ParamType.PDef);
                        break;
                    case Slot.helm:
                        MainParameterIcon.sprite = DataBaseController.Instance.ParameterIcon(ParamType.MDef);
                        break;
                }
                var secondary = HeroShopRandomItem.GetSecondaryParam(totalPoints, recipeItem.recipeSlot);
                var prm = Instantiate(PrefabSecondaryParam);
                var img2 = prm.GetComponent<Image>();
                var filed = prm.GetComponent<Text>();
                var minS = secondary.Value*0.5f;
                var maxS = secondary.Value;
                string info = "";
                Sprite spr = null;
                if (type.HasValue)
                {
                    switch (type.Value)
                    {
                        case CatalysItemType.red:
                            minS *= 1.25f;
                            minS *= 1.25f;
                            info = minS.ToString("0") + " - " + maxS.ToString("0");
                            break;
                        case CatalysItemType.blue:
                            break;
                        case CatalysItemType.green:
                            break;
                        case CatalysItemType.black:
                            minS *= 1f;
                            minS *= 1.5f;
                            info = minS.ToString("0") + " - " + maxS.ToString("0");
                            break;
                        case CatalysItemType.white:
                            info = "cost x2";
                            spr = DataBaseController.Instance.ItemIcon(ItemId.money);
                            break;
                    }
                }
                else
                {
                    info = minS.ToString("0") + " - " + maxS.ToString("0");
                    spr = DataBaseController.Instance.ParameterIcon(secondary.Key);
                }

                filed.text = info;
                img2.sprite = spr;
                img2.transform.SetParent(LayoutSpecials, false);

                break;
            case Slot.Talisman:
                break;
        }
    }
}

