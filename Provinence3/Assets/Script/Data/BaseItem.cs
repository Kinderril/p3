using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class BaseItem
{

    protected const char DELEM = '|';
    protected const char DPAR = '{';
    protected const char MDEL = '>';
    public string icon = "noicon";
    public string name = "no name";
    protected bool isEquped;
    public Sprite IconSprite;
    public int cost;
    public Slot Slot;


    public abstract char FirstChar();
    public abstract void Activate(Hero hero);
    public abstract string Save();
    public bool IsEquped
    {
        get { return isEquped; }
        set
        {
            isEquped = value;
            Save();
        }
    }


    public virtual void LoadTexture()
    {
    }
}

