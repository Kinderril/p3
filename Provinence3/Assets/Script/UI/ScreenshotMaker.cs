using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


public class ScreenshotMaker
{
    private const int MAX_SCREENS = 4;
    private int currentIndex = 0;
    private List<Sprite> list;
    private Camera mainCam;
    

    public ScreenshotMaker()
    {
        Load();
    }

    private string GetPath(int marker)
    {
        return Application.persistentDataPath + "/screen" + marker + ".png";
    }

    public List<Sprite> GetRandomScreenshots(int count)
    {
        var list = new List<Sprite>();
        return list;
    } 

    private void Load()
    {
        for (int i = 0; i < MAX_SCREENS; i++)
        {
            var path = GetPath(i);
            if (File.Exists(path))
            {
                var bytes = System.IO.File.ReadAllBytes(path);
                var texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                texture.filterMode = FilterMode.Bilinear;
                texture.Apply();
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                list.Add(sprite)
            }
        }
    }

    public void MakeScreenshot()
    {
        RenderTexture render = new RenderTexture(500,300,0,RenderTextureFormat.ARGB32);
        var lst = RenderCam.RenderImpl(mainCam, render, GetPath(currentIndex));
        var sprite = Sprite.Create(lst, new Rect(0, 0, lst.width, lst.height), new Vector2(0.5f, 0.5f));
        list.Add(sprite);
        currentIndex++;
        if (currentIndex >= MAX_SCREENS)
        {
            currentIndex = 0;
        }
    }
}

