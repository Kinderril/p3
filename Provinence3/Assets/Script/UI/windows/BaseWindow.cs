using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseWindow : MonoBehaviour
{
    public Transform TopPanel;
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Init()
    {
        gameObject.SetActive(true);
    }

    protected void ClearTransform(Transform go)
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

