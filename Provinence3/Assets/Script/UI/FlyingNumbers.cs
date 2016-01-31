using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public enum FlyNumerDirection
{
    side,
    non,
}
public class FlyingNumbers : PoolElement
{
    public Text text;
    public Image image;
    private Action OnDead;
    private Animator anim;
    private const string keyLeft = "left";
    private const string keyRight = "right";
    private const string keyNone = "none";

    public void Init(string msg,Color textColor, FlyNumerDirection flyDir = FlyNumerDirection.side,int size = 42,Action OnDead = null)
    {
        base.Init();
        this.OnDead = OnDead;
        text.text = msg;
        text.fontSize = size;
        text.color = textColor;
        subInit(flyDir);
    }

    private void subInit(FlyNumerDirection flyDir)
    {
        anim = GetComponent<Animator>();
        switch (flyDir)
        {
            case FlyNumerDirection.side:
                if (UnityEngine.Random.Range(0, 100) > 50)
                {
                    anim.SetTrigger(keyLeft);
                }
                else
                {
                    anim.SetTrigger(keyRight);
                }
                break;
            case FlyNumerDirection.non:
                anim.SetTrigger(keyNone);
                break;
        }
    }

    public void Init(string txt, Color textColor,  Sprite spr, FlyNumerDirection flyDir = FlyNumerDirection.side, int size = 42, Action OnDead = null)
    {
        base.Init();
        this.OnDead = OnDead;
        text.text = txt;
        text.fontSize = size;
        text.color = textColor;
        image.sprite = spr;
        subInit(flyDir);
    }

    public void EndAnimation()
    {
        if (OnDead != null)
        {
            OnDead();
        }
        else
        {
            EndUse();
        }
    }
}
