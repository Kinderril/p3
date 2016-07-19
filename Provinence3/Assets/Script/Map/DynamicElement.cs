using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DynamicElement : MonoBehaviour
{
    private const int RESET_DIST_SQR = 200;
    private float sqrDist;
    private Hero hero;
    public void Init(Hero hero)
    {
        this.hero = hero;
        var ang = UnityEngine.Random.Range(0, 360);
        var toRotate = new Vector3(30,0,0);
        var sp = Quaternion.AngleAxis(ang, Vector3.up) * toRotate;
        transform.position = hero.transform.position + sp;
    }

    public void UpdateByMap()
    {
        var diff = hero.transform.position - transform.position;
        sqrDist = diff.sqrMagnitude;
        if (sqrDist > RESET_DIST_SQR)
        {

            ResetPosition(diff);
        }
    }

    private void ResetPosition(Vector3 diff)
    {
//        var oldp = transform.position;
        transform.position = transform.position + diff*1.9f;
//        Debug.Log("Reset:" + name + "  diff:" + diff + "   to:"+ transform.position + "   from:" + oldp);
        Utils.GroundTransform(transform);

    }
}

