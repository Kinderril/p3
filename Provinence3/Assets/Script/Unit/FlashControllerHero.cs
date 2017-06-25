using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FlashControllerHero : FlashController
{
    private float startVal = 3.5f;
    private float endVal = -1f;
    private const string keyTime = "_RimPower";

    protected override void AwakeInner()
    {
        endVal *= 2;
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
    }

    protected override void UpdateInner()
    {
        if (isPlaying)
        {
            curTime += Time.deltaTime;
            float c = curTime;
            if (curTime > FlashController.TOTAL_FLASH_TIME / 2f)
            {
                c = FlashController.TOTAL_FLASH_TIME - curTime;
            }
            var percent = c/FlashController.TOTAL_FLASH_TIME;
            var pp = startVal*(1 - percent) + endVal*percent;
//                        Debug.Log("percent:" + percent + "   p:" + pp);
            c = Mathf.Clamp(pp, -5f, Single.MaxValue);
            foreach (var material in materials)
            {
                material.SetFloat(keyTime, c);
            }
            //            Debug.Log("curTime:" + curTime + "   c:"+c);
            if (curTime > FlashController.TOTAL_FLASH_TIME)
            {
                End();

            }
        }
    }
    
    protected override void SubPlaye()
    {
        foreach (var material in materials)
        {
            material.SetFloat(keyTime, curTime);
        }
    }

    void End()
    {
        enabled = false;
        isPlaying = false;
    }

}

