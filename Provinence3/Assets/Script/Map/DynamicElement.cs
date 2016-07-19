using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class DynamicElement : MonoBehaviour
{
    private const int RESET_DIST_SQR = 63;
    private float sqrDist;
    private Hero hero;
    public void Init(Hero hero)
    {
        this.hero = hero;

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
        diff.y = 1000;
        transform.position = transform.position + diff;
        Utils.GroundTransform(transform);

    }
}

