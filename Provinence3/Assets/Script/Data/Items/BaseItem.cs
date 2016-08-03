using System;
using System.Collections.Generic;
using System.IO;
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
    public int Id;


    public abstract char FirstChar();
    public abstract void Activate(Hero hero,Level lvl);
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

    public virtual string Name
    {
        get { return name; }
    }


    public virtual void LoadTexture()
    {
    }

    public virtual void Clear()
    {
        if (File.Exists(icon))
        {
            File.Delete(icon);
        }
    }
}

