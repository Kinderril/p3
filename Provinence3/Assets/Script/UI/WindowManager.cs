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
        }
        ConfirmWindow.gameObject.SetActive(false);
        InfoWindow.gameObject.SetActive(false);
    }
    public void OpenWindow(MainState state)
    {
        Debug.Log("OpenWindow " + state);
        var isInGame = state == MainState.play;
        MainCamera.enabled = isInGame;
        SubCamera.enabled = !isInGame;
        if (currentWindow != null)
        {
            currentWindow.Close();
        }
        var window = windows.FirstOrDefault(x => x.state == state).window;
        //window.StartAnimation();
        window.Init();
        currentWindow = window;
    }

   
}

