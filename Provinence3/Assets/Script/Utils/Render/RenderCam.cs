using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public enum RenderSlotType
{
    back,
    bot,mid,top,
    frame
}

public class RenderCam : Singleton<RenderCam>
{

    public Camera RenderCamera;
    public Image backImage;
    public Image topImage;
    public Image midImage;
    public Image botImage;
    public Image frameImage;
    private RenderTexture RenderTexture;
    private int lastId;
    private const string key = "lastIDKey";
    private string baseway = "sprites/Construct/";

    void Awake()
    {
        lastId = PlayerPrefs.GetInt(key, 0);
        RenderCamera.gameObject.SetActive(false);
    } 

    public Texture2D DoRender(Slot slot,out string icon)
    {
        RenderCamera.gameObject.SetActive(true);
        RenderImage(slot, RenderSlotType.back, backImage);
        RenderImage(slot, RenderSlotType.bot, botImage);
        RenderImage(slot, RenderSlotType.mid, midImage);
        RenderImage(slot, RenderSlotType.top, topImage);
        RenderImage(slot, RenderSlotType.frame, frameImage);

        var fileName = Application.persistentDataPath + "/imagePic" + lastId + ".png";
        var pngTex = RenderImpl(RenderCamera, RenderTexture,fileName);
        lastId++;
        PlayerPrefs.SetInt(key, lastId);
        icon = fileName;
        return pngTex;
    }

    public static Texture2D RenderImpl(Camera RenderCamera, RenderTexture RenderTexture, string fileName)
    {
        var pngTex = new Texture2D(RenderCamera.pixelWidth, RenderCamera.pixelHeight);
        RenderTexture = new RenderTexture(RenderCamera.pixelWidth, RenderCamera.pixelHeight, 24);
        RenderCamera.targetTexture = RenderTexture;
        RenderCamera.Render();
        RenderTexture.active = RenderTexture;
        var rect = new Rect(0, 0, RenderCamera.pixelWidth, RenderCamera.pixelHeight);
        pngTex.ReadPixels(rect, 0, 0);
        //        RenderCamera.targetTexture = null;
        var bytes = pngTex.EncodeToPNG();
        StreamWriter fileStream;
        fileStream = File.CreateText(fileName);
        fileStream.Close();
        File.WriteAllBytes(fileName, bytes);
        Debug.Log("Render done:" + fileName);
        return pngTex;
    }

    private void RenderImage(Slot slot, RenderSlotType type,Image img)
    {
        int randBot = UnityEngine.Random.Range(0, 3);
        string path = "";
        switch (type)
        {
            case RenderSlotType.back:
                path = baseway + type.ToString() + "/" + randBot;
                break;
            case RenderSlotType.bot:
            case RenderSlotType.mid:
            case RenderSlotType.top:
                path = baseway + slot.ToString() + "/" + type.ToString() + "/" + randBot;
                break;
            case RenderSlotType.frame:
                path = baseway +  "frame/" + randBot;

                break;
        }
        
        var res = Resources.Load<Sprite>(path);
        img.sprite = res;

    }
}
