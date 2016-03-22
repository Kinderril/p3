﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseWindow : MonoBehaviour
{
    public Transform TopPanel;
    private Animator animator;
    private const string open_key = "open";
    private const string close_key = "close";
    private CanvasGroup canvasGroup;
    void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }



    public virtual void Close()
    {
        if (animator != null)
        {
            animator.SetTrigger(close_key);
            canvasGroup.interactable = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void Init()
    {
        gameObject.SetActive(true);
        if (animator != null)
        {
            canvasGroup.interactable = false;
            animator.SetTrigger(open_key);
        }
//        else
//        {
//            gameObject.SetActive(true);
//        }
    }

    public void EndClose()
    {

        gameObject.SetActive(false);
    }

    public void EndOpen()
    {

        canvasGroup.interactable = true;
    }

    public static void ClearTransform(Transform go)
    {
        foreach (Transform child in go.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public virtual void OnToMission()
    {
        WindowManager.Instance.OpenWindow(MainState.mission);
    }
    public virtual void OnToShop()
    {
        WindowManager.Instance.OpenWindow(MainState.shop);
    }
    public virtual void OnToStart()
    {
        WindowManager.Instance.OpenWindow(MainState.start);
    }
    public virtual void OnToParameters()
    {
        WindowManager.Instance.OpenWindow(MainState.parameters);
    }
    public virtual void OnToPause()
    {
        WindowManager.Instance.OpenWindow(MainState.pause);
    }
    public virtual void OnToPlay()
    {
        WindowManager.Instance.OpenWindow(MainState.play);
    }
    public virtual void OnToEnd()
    {
        WindowManager.Instance.OpenWindow(MainState.end);
    }
    
}

