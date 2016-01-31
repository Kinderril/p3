using UnityEngine;

public class MyGlobalFog : MonoBehaviour
{
    //fog script
    public enum FogMode
    {
        AbsoluteYAndDistance = 0,
        AbsoluteY = 1,
        Distance = 2,
        RelativeYAndDistance = 3
    }

    public bool bEnableFog = true;
    private float CAMERA_ASPECT_RATIO = 1.333333f;
    private float CAMERA_FAR = 50.0f;
    private float CAMERA_FOV = 60.0f;
    private float CAMERA_NEAR = 0.5f;
    private Material fogMaterial;
    public FogMode fogMode = FogMode.AbsoluteYAndDistance;
    public Shader fogShader;
    public float globalDensity = 1.0f;
    public Color globalFogColor = Color.grey;
    public float height = 0.0f;
    public float heightScale = 100.0f;
    public float startDistance = 200.0f;
    private Camera camera;

    void Awake()
    {
        camera = MainController.Instance.MainCamera;
    }
    private void Start()
    {
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        DrawFog(source, destination);
    }

    private void DrawFog(RenderTexture source, RenderTexture destination)
    {
        if (!bEnableFog)
        {
            Graphics.Blit(source, destination);
            return;
        }
        if (fogMaterial == null)
        {
            fogMaterial = new Material(fogShader);
        }
        CAMERA_NEAR = camera.nearClipPlane;
        CAMERA_FAR = camera.farClipPlane;
        CAMERA_FOV = camera.fieldOfView;
        CAMERA_ASPECT_RATIO = camera.aspect;

        var frustumCorners = Matrix4x4.identity;
        //Vector4 vec;
        //Vector3 corner;

        var fovWHalf = CAMERA_FOV*0.5f;

        var toRight = camera.transform.right*CAMERA_NEAR*Mathf.Tan(fovWHalf*Mathf.Deg2Rad)*CAMERA_ASPECT_RATIO;
        var toTop = camera.transform.up*CAMERA_NEAR*Mathf.Tan(fovWHalf*Mathf.Deg2Rad);

        var topLeft = (camera.transform.forward*CAMERA_NEAR - toRight + toTop);
        var CAMERA_SCALE = topLeft.magnitude*CAMERA_FAR/CAMERA_NEAR;

        topLeft.Normalize();
        topLeft *= CAMERA_SCALE;

        var topRight = (camera.transform.forward*CAMERA_NEAR + toRight + toTop);
        topRight.Normalize();
        topRight *= CAMERA_SCALE;

        var bottomRight = (camera.transform.forward*CAMERA_NEAR + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= CAMERA_SCALE;

        var bottomLeft = (camera.transform.forward*CAMERA_NEAR - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= CAMERA_SCALE;

        frustumCorners.SetRow(0, topLeft);
        frustumCorners.SetRow(1, topRight);
        frustumCorners.SetRow(2, bottomRight);
        frustumCorners.SetRow(3, bottomLeft);

        fogMaterial.SetMatrix("_FrustumCornersWS", frustumCorners);
        fogMaterial.SetVector("_CameraWS", camera.transform.position);
        fogMaterial.SetVector("_StartDistance", new Vector4(1.0f/startDistance, (CAMERA_SCALE - startDistance)));
        fogMaterial.SetVector("_Y", new Vector4(height, 1.0f/heightScale));

        fogMaterial.SetFloat("_GlobalDensity", globalDensity*0.01f);
        fogMaterial.SetColor("_FogColor", globalFogColor);
        CustomGraphicsBlit(source, destination, fogMaterial, (int) fogMode);
    }

    private static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
    {
        Graphics.Blit(source, dest, fxMaterial, passNr);
        /*
        RenderTexture.active = dest;
 
        fxMaterial.SetTexture("_MainTex", source);
 
        GL.PushMatrix();
        GL.LoadOrtho();
 
        fxMaterial.SetPass(passNr);
 
        GL.Begin(GL.QUADS);
 
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f); // BL
 
        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f); // BR
 
        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f); // TR
 
        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f); // TL
 
        GL.End();
        GL.PopMatrix();
        */
    }
}