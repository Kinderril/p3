using System.Collections.Generic;
using UnityEngine;

public class FlashControllerHero : FlashController
{
    public float endVal = -1f;
    public string keyTime = "_RimPower";
    public float startVal = 3.5f;

    protected override void AwakeInner()
    {
        endVal *= 2;
        foreach (var renderer1 in Renderers)
        {
            var materialsInside = new List<Material>();
            var mat = renderer1.materials;
            for (var i = 0; i < mat.Length; i++)
            {
                materialsInside.Add(Instantiate(mat[i]));
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
            var c = curTime;
            if (curTime > TOTAL_FLASH_TIME/2f)
            {
                c = TOTAL_FLASH_TIME - curTime;
            }
            var percent = 2f*c/TOTAL_FLASH_TIME;
            var pp = startVal*(1 - percent) + endVal*percent;
            c = Mathf.Clamp(pp, -5f, float.MaxValue);
            foreach (var material in materials)
            {
                material.SetFloat(keyTime, c);
            }
//            Debug.Log("setting balue:" + c + "  materials:" + materials.Count + "   startVal:" + startVal + " endVal:" +
//                      endVal + "  percent:" + percent);
            if (curTime > TOTAL_FLASH_TIME)
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

    private void End()
    {
        enabled = false;
        isPlaying = false;
    }
}