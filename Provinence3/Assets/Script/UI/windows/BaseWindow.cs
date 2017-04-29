using System;
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
    private bool isAnimating;
    public bool playOpenAnimIfCan = true;

    public Animator Animator
    {
        get { return animator; }
    }

    public void Activate()
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
        Init<object>(null);
    }

    public virtual void Init<T>(T obj)
    {
        bool isAnim = animator != null && playOpenAnimIfCan;  
        if (isAnim)
        {
            transform.localPosition = new Vector2(-1300, 00);
        }
        else
        {
            transform.localPosition =Vector3.zero;
            if (canvasGroup != null)
                canvasGroup.interactable = true;
        }
        gameObject.SetActive(true);
        if (isAnim)
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
        WindowManager.Instance.OpenWindow<object>(MainState.mission,null);
    }
    public virtual void OnToLoadingScreen()
    {
        WindowManager.Instance.OpenWindow<object>(MainState.loading, null);
    }
    public virtual void OnToShop()
    {
        WindowManager.Instance.OpenWindow<object>(MainState.shop, null);
    }
    public virtual void OnToStart()
    {
        WindowManager.Instance.OpenWindow<object>(MainState.start, null);
    }
    public virtual void OnToParameters()
    {
        WindowManager.Instance.OpenWindow<object>(MainState.parameters, null);
    }
    public virtual void OnToStatistics()
    {
        WindowManager.Instance.OpenWindow<object>(MainState.statistics, null);
    }
//    public virtual void OnToPause()
//    {
//        WindowManager.Instance.OpenWindow<object>(MainState.pause, null);
//    }
    public virtual void OnToPlay()
    {
        WindowManager.Instance.OpenWindow<object>(MainState.play, null);
    }
    public virtual void OnToEnd()
    {
        WindowManager.Instance.OpenWindow<object>(MainState.end, null);
    }
    
}

