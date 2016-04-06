using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct WindowT
{
    public MainState state;
    public BaseWindow window;
}

public class WindowManager : Singleton<WindowManager>
{
    public WindowT[] windows;
    private BaseWindow currentWindow;
    private BaseWindow nextWindow;

    public Camera MainCamera;
    public Camera SubCamera;
    public ConfirmWindow ConfirmWindow;
    public InfoWindow InfoWindow;


    public BaseWindow CurrentWindow
    {
        get { return currentWindow; }
    }

    public void Init()
    {
        foreach (var window in windows)
        {
            window.window.gameObject.SetActive(false);
            window.window.Activate();
        }
        ConfirmWindow.gameObject.SetActive(false);
        InfoWindow.gameObject.SetActive(false);
    }

    public void OpenWindow(MainState state)
    {
        OpenWindow<object>(state, null);
    }

    public void OpenWindow<T>(MainState state,T obj) 
    {
        Debug.Log("OpenWindow " + state);
        var isInGame = state == MainState.play;
        MainCamera.enabled = isInGame;
        SubCamera.enabled = !isInGame;
        var nextWindow = windows.FirstOrDefault(x => x.state == state).window;
        if (currentWindow != null)
        {
            if (nextWindow.Animator == null)
            {
                CurrentWindow.EndClose();
            }
            else
            {
                currentWindow.Close();
            }
            var sIndex = currentWindow.transform.GetSiblingIndex();
            nextWindow.transform.SetSiblingIndex(sIndex + 1);
        }
        //window.StartAnimation();
        if (obj != null)
        {
            nextWindow.Init<T>(obj);
        }
        else
        {
            nextWindow.Init();
        }
        currentWindow = nextWindow;
    }

   
}

