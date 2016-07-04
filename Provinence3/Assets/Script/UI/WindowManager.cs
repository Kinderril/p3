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
    private GameObject planeX;
    public GameObject MainBack;

    public Transform UIPool;

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


    private void MakeScreenShot()
    {
        var cam = MainCamera;
        planeX = new GameObject("planeX");
        var mr = planeX.AddComponent<MeshRenderer>();
        var mf = planeX.AddComponent<MeshFilter>();
        var mesh = new Mesh();
        cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3[] vertices =
        {
            cam.ViewportToWorldPoint(new Vector3(0, 0, 0)),
            cam.ViewportToWorldPoint(new Vector3(1, 0, 0)),
            cam.ViewportToWorldPoint(new Vector3(0, 1, 0)),
            cam.ViewportToWorldPoint(new Vector3(1, 1, 0))
        };
        mesh.vertices = vertices;
        Vector2[] uv = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.uv = uv;
        int[] triangles = { 2, 1, 0, 2, 3, 1 };
        mesh.triangles = triangles;
        mf.mesh = mesh;
//        mr.material = new Material(Shader.Find("Custom/BackgroundNoAlpha"));
        var rt = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture renderTextureTmp = null;
        if (cam.targetTexture != null)
        {
            renderTextureTmp = cam.targetTexture;
        }
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;
        if (renderTextureTmp != null)
        {
            cam.targetTexture = renderTextureTmp;
        }
        else
        {
            cam.targetTexture = null;
        }
        mr.material.mainTexture = rt;
    }

    public void OpenWindow<T>(MainState state,T obj)
    {
        if (planeX != null)
        {
            Destroy(planeX.gameObject);
        }
        if (state == MainState.end)
        {
            MakeScreenShot();
        }
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

