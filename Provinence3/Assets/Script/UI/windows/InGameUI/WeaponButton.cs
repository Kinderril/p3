using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WeaponButton : MonoBehaviour
{
    public Weapon weapon;
    private bool isOpen = false;
    public void Init(Weapon weapon)
    {
        this.weapon = weapon;
        isOpen = true;
    }

    public void Lock()
    {
        isOpen = false;
    }

    public void Select()
    {
        if (isOpen)
        {
           // MainController.Instance.level.MainHero.ChangeWeaponTo(weapon);

        }
    }
    
}

