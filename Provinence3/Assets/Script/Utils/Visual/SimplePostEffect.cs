using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class SimplePostEffect : PostEffectsBase
{
//    public Shader TintShader = null;
    public Color TintColour;
    public Material TintMaterial = null;
    public Material BloomMaterial = null;
    public Material BlurMaterial = null;
    public Camera camera;
    public int descale = 4;

    public RenderTexture renderTexture;
    public RenderTexture renderTextureMinor;
    public RenderTexture renderTextureAfterBlur;
    public RenderTexture rayMarchRT;

    public NsRenderManager NsRenderManager;
//    private Rect rect = new Rect(0f, 0f, 200f, 200f);

    void Awake()
    {
        //        renderTexture.
        //        renderTexture.width = rayMarchRT.width = Screen.width;
        //        renderTexture.height = rayMarchRT.height = Screen.height;
        renderTexture = new RenderTexture(Screen.width, Screen.height, 1, RenderTextureFormat.ARGBHalf);
        renderTextureAfterBlur = new RenderTexture(Screen.width, Screen.height, 1, RenderTextureFormat.ARGBHalf);

        rayMarchRT = new RenderTexture(Screen.width, Screen.height, 1, RenderTextureFormat.ARGBHalf);
        renderTextureMinor = new RenderTexture(Screen.width/ descale, Screen.height/ descale, 1,RenderTextureFormat.ARGBHalf);
        //        rect = new Rect(0f, 0f, Screen.width, Screen.height);
        BloomMaterial.SetTexture("_SubTex", renderTextureAfterBlur);
        BloomMaterial.SetTexture("_MainTex", renderTexture);
    }

    public override bool CheckResources()
    {
        return true;
    }

//    RenderTexture mainSceneRT;
    void OnPreRender()
    {
        NsRenderManager.PreRenderOut();
        camera.targetTexture = renderTexture;
    }

    void OnPostRender()
    {
        //        BloomMaterial.SetPass()
        NsRenderManager.PostRenderOut();
        Graphics.Blit(renderTexture, renderTextureMinor);

//        Graphics.Blit(renderTextureMinor, renderTextureAfterBlur, BlurMaterial);

        //        BloomMaterial.SetPass(0);

        // render to secondary render target
        Graphics.Blit(renderTexture, rayMarchRT, BloomMaterial);
//         You have to set target texture to null for the Blit below to work
        camera.targetTexture = null;
        Graphics.Blit(rayMarchRT, null as RenderTexture);
        
    }
}


