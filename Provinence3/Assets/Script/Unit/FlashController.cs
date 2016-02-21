using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashController : MonoBehaviour
{
    private const float totalTime = .17f;
    private float curTime = 0;
    private Shader flashShader;
    private Shader simpleShader;
//    public Material material;
    private Renderer Renderer;
    private bool isPlaying = false;
    private const string keyTime = "_Time2";
    private List<Material> materials = new List<Material>(); 

    void Awake()
    {
//        if (material == null)
//            material = GetComponent<Material>();


        Renderer = GetComponent<Renderer>();
        var mat = Renderer.materials;
        for (int i = 0; i < mat.Length; i++)
        {
            var m = Instantiate(mat[i]) as Material;
            materials.Add(m);
        }
        Renderer.materials = materials.ToArray();

        //        Renderer.material = Instantiate(Renderer.material) as Material;


        flashShader = DataBaseController.Instance.flashShader;
        simpleShader = DataBaseController.Instance.simpleShader;
    }

    public void Play()
    {
        enabled = true;
        isPlaying = true;
        curTime = 0;

        foreach (var material in materials)
        {
            material.shader = flashShader;
            material.SetFloat(keyTime, curTime);

        }

    }


    void Update()
    {
        if (isPlaying)
        {
            curTime += Time.deltaTime;
            float c = curTime;
            if (curTime > totalTime/2f)
            {
                c = totalTime - curTime;
            }
//            Debug.Log("curTime:" + curTime + "   prev c:" + c);
            c = 2*c/totalTime;
            foreach (var material in materials)
            {
                material.SetFloat(keyTime, c);
            }
//            Debug.Log("curTime:" + curTime + "   c:"+c);
            if (curTime > totalTime)
            {
                End();

            }
        }
    }

    void End()
    {
        foreach (var material in materials)
        {
            material.shader = simpleShader;
        }
        enabled = false;
        isPlaying = false;
    }

}
