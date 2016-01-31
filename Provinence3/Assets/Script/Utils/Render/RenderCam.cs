using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public enum RenderSlotType
{
    back,
    bot,mid,top
}

public class RenderCam : Singleton<RenderCam>
{

    public Camera RenderCamera;
    public Image backImage;
    public Image topImage;
    public Image midImage;
    public Image botImage;
    private RenderTexture RenderTexture;
    private int lastId;
    private const string key = "lastIDKey";
    private string baseway = "sprites/Construct/";

    void Awake()
    {
        lastId = PlayerPrefs.GetInt(key, 0);
        RenderCamera.gameObject.SetActive(false);
    } 

    public string DoRender(Slot slot )
    {
        RenderCamera.gameObject.SetActive(true);
        RenderImage(slot, RenderSlotType.back, backImage);
        RenderImage(slot, RenderSlotType.bot, botImage);
        RenderImage(slot, RenderSlotType.mid, midImage);
        RenderImage(slot, RenderSlotType.top, topImage);
        //        RenderTexture rendText = RenderTexture.active;



        RenderCamera.gameObject.SetActive(true);
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
        var fileName = Application.persistentDataPath + "/imagePic" + lastId + ".png";
        fileStream = File.CreateText(fileName);
        fileStream.Close();
        File.WriteAllBytes(fileName, bytes);
        RenderCamera.gameObject.SetActive(false);










//        RenderTexture.active = RenderCamera.targetTexture;
//
//        Texture2D cameraImage = new Texture2D(RenderCamera.targetTexture.width, RenderCamera.targetTexture.height, TextureFormat.RGB24, false);
//        cameraImage.ReadPixels(new Rect(0, 0, RenderCamera.targetTexture.width, RenderCamera.targetTexture.height), 0, 0);
//        cameraImage.Apply();
//        RenderTexture.active = rendText;
        
//        byte[] bytes = cameraImage.EncodeToPNG();

//        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("Render done:" + fileName);
        lastId++;
        PlayerPrefs.SetInt(key, lastId);
//        RenderCamera.gameObject.SetActive(false);
        return fileName;
    }

    private void RenderImage(Slot slot, RenderSlotType type,Image img)
    {
        int randBot = UnityEngine.Random.Range(0, 1);
        string path;
        if (type == RenderSlotType.back)
        {
            path = baseway + type.ToString() + "/" + randBot;
        }
        else
        {
            path = baseway + slot.ToString() + "/" + type.ToString() + "/" + randBot;
        }
        var res = Resources.Load<Sprite>(path);
        img.sprite = res;

    }
}
