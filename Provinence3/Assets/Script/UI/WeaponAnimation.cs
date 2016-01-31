using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WeaponAnimation : MonoBehaviour
{
    private WeaponChooser weaponChooser;
    public void Open()
    {
        weaponChooser.AnimationOpenEnd();
    }

    public void Close()
    {
        weaponChooser.AnimationCloseEnd();
    }

    public void Init(WeaponChooser weaponChooser)
    {
        this.weaponChooser = weaponChooser;
    }
}

