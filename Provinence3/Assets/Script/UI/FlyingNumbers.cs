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
    public Text subText;
    public Image image;
    private Action OnDead;
    private Animator anim;
    private LayoutGroup LayoutGroup;
    private const string keyLeft = "left";
    private const string keyRight = "right";
    private const string keyNone = "none";

    void Awake()
    {
        LayoutGroup = GetComponent<LayoutGroup>();
    }

    public void Init(string msg,string sub,Color textColor, FlyNumerDirection flyDir = FlyNumerDirection.side,int size = 42,Action OnDead = null)
    {
        base.Init();
        this.OnDead = OnDead;
        text.text = msg;
        text.fontSize = size;
        if (image != null)
            image.enabled = false;
        if (subText != null)
        {
            var haveSub = sub.Length > 0;
            if (haveSub)
            {
                subText.text = sub;
                subText.gameObject.SetActive(haveSub);
            }
            else
            {
                if (LayoutGroup != null)
                {
                    LayoutGroup.padding = new RectOffset(0,0,0,0);
                }
            }
        }

        text.color = textColor;
        subInit(flyDir);
    }
    public void Init(string msg,Color textColor, FlyNumerDirection flyDir = FlyNumerDirection.side,int size = 42,Action OnDead = null)
    {
        base.Init();
        this.OnDead = OnDead;
        if (image != null)
            image.enabled = false;
        text.text = msg;
        text.fontSize = size;
        text.color = textColor;
        if (subText != null)
        {
            subText.gameObject.SetActive(false);
        }
        subInit(flyDir);
    }

    private void subInit(FlyNumerDirection flyDir)
    {
//        transform.localScale = Vector3.one;
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

    public void Init(string txt, Color textColor,  Sprite spr , FlyNumerDirection flyDir = FlyNumerDirection.side, int size = 42, Action OnDead = null)
    {
        base.Init();
        this.OnDead = OnDead;
        text.text = txt;
        text.fontSize = size;
        text.color = textColor;
        if (spr == null)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            image.sprite = spr;
        }
        if (subText != null)
        {
            subText.gameObject.SetActive(false);
        }
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
