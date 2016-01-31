using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WeaponChooserView : MonoBehaviour
{
    public List<WeaponButtonView> views = new List<WeaponButtonView>();
    private WeaponButtonView currentSelected;
    private Dictionary<Weapon, WeaponButtonView> dictionary; 

    public void Init()
    {
        var hero  = MainController.Instance.level.MainHero;
        dictionary = new Dictionary<Weapon, WeaponButtonView>();
        foreach (var v in hero.InventoryWeapons)
        {
            var linked = views.FirstOrDefault(x => x.WeaponType == v.Parameters.type);
            if (linked != null)
            {
                dictionary.Add(v, linked);
                linked.Select(false);
            }
            else
            {
                Debug.LogError("can't load all weapons to UI");
            }
        }
        SetWeapon(hero.curWeapon);
    }


    public void SetWeapon(Weapon weapon)
    {
        if (currentSelected != null)
            currentSelected.Select(false);
        currentSelected = dictionary[weapon];
        currentSelected.Select(true);
    }
}

