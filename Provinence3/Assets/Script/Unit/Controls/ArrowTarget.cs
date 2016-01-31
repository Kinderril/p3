using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ArrowTarget : MonoBehaviour
{
    private BossUnit BossUnit;
    public int distToDisappear;
    private bool isActive;
    public GameObject ArrowGameObject;

    void Awake()
    {
        ArrowGameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (BossUnit != null)
        {
            if (isActive)
            {
                ArrowGameObject.transform.LookAt(BossUnit.transform);
            }
            isActive = BossUnit.mainHeroDist > distToDisappear;
            ArrowGameObject.SetActive(isActive);
        }
    }

    public void Init(BossUnit boss)
    {
        BossUnit = boss;
        ArrowGameObject.SetActive(true);
        gameObject.SetActive(true);
    }
}

