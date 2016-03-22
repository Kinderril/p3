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
        bool isAnim = animator != null;
        if (isAnim)
        {
            transform.localPosition = new Vector2(-1300, 00);
        }
        else
        {
            transform.localPosition =Vector3.zero;
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

