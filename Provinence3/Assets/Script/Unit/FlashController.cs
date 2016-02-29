using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashController : MonoBehaviour
{
    private const float totalTime = .26f;
    private float power = 14;
    private float curTime = 0;
    private Shader flashShader;
    private Shader simpleShader;
    public List<Renderer> Renderers;
    private bool isPlaying = false;
    private const string keyTime = "_Time2";
    private List<Material> materials = new List<Material>(); 

    void Awake()
    {
        foreach (var renderer1 in Renderers)
        {
            List<Material> materialsInside = new List<Material>();
            var mat = renderer1.materials;
            for (int i = 0; i < mat.Length; i++)
            {
                materialsInside.Add(Instantiate(mat[i]) as Material);
            }
            renderer1.materials = materialsInside.ToArray();
            materials.AddRange(materialsInside);
        }
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
            c = Mathf.Clamp(power * c/totalTime,0,Single.MaxValue);
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
//        Debug.Log("End:" );
        foreach (var material in materials)
        {
            material.shader = simpleShader;
        }
        enabled = false;
        isPlaying = false;
    }

}
