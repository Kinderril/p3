using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlashController : MonoBehaviour
{
    public const float TOTAL_FLASH_TIME = .26f;
    private const string keyTime = "_Time2";

    private float power = 14;
    private Shader flashShader;
    private Shader simpleShader;

    protected float curTime = 0;
    protected bool isPlaying = false;
    public List<Renderer> Renderers;
    protected List<Material> materials = new List<Material>(); 

    void Awake()
    {
        AwakeInner();
    }

    protected virtual void AwakeInner()
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

        SubPlaye();
    }

    protected virtual void SubPlaye()
    {
        foreach (var material in materials)
        {
            material.shader = flashShader;
            material.SetFloat(keyTime, curTime);
        }

    }

    protected virtual void UpdateInner()
    {

        if (isPlaying)
        {
            curTime += Time.deltaTime;
            float c = curTime;
            if (curTime > TOTAL_FLASH_TIME / 2f)
            {
                c = TOTAL_FLASH_TIME - curTime;
            }
            //            Debug.Log("curTime:" + curTime + "   prev c:" + c);
            c = Mathf.Clamp(power * c / TOTAL_FLASH_TIME, 0, Single.MaxValue);
            foreach (var material in materials)
            {
                material.SetFloat(keyTime, c);
            }
            //            Debug.Log("curTime:" + curTime + "   c:"+c);
            if (curTime > TOTAL_FLASH_TIME)
            {
                End();

            }
        }
    }

    void Update()
    {
        UpdateInner();
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
