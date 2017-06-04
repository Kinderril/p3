﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpellItem : BaseItem, IEnhcant
{
    public const char FIRSTCHAR = '♝';
    public BaseSpell SpellData;
    public int enchant;

    public SpellItem(BaseSpell spellData)
    {
        SpellData = spellData;
        LoadTexture();
    }

    public override char FirstChar()
    {
        return FIRSTCHAR;
    }

    public override void Activate(Hero hero, Level lvl)
    {

    }

    public override string Name
    {
        get { return SpellData.Name.ToString(); }
    }

    public override void LoadTexture()
    {
        IconSprite = DataBaseController.Instance.SpellIcon(SpellData.IdIcon);
    }

    public override string Save()
    {
        var s = Id.ToString() + MDEL + SpellData.Id.ToString() + MDEL + enchant.ToString();
        return s;
    }

    public BaseItem BaseItem
    {
        get { return this; }
    }
    public void Enchant(int sum)
    {
        enchant = Mathf.Clamp(enchant + sum, 0, 30);
    }
    public static SpellItem Create(string subStr)
    {
        Debug.Log("TalismanItem Creat from:   " + subStr);
        var pp = subStr.Split(MDEL);
        var Id = Convert.ToInt32(pp[0]);
        var IdSpell = Convert.ToInt32(pp[1]);
        var enchant = Convert.ToInt32(pp[2]);
        var spellData = SpellsDataBase.Spells[IdSpell];
        SpellItem spellItem = new SpellItem(spellData);
        spellItem.Id = Id;
        spellItem.enchant = enchant;
        return spellItem;
    }
}

