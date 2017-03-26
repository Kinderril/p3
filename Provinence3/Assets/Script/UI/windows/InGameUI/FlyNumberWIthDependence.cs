﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FlyNumberWIthDependence : PoolElement
{
    public FlyingNumbers incFlyingNumbers;
    private Transform dependence;
    private Camera cam;

    public static FlyNumberWIthDependence Create(Transform tr,string str)
    {
        var fn = DataBaseController.Instance.Pool.GetItemFromPool<FlyNumberWIthDependence>(PoolType.flyNumberInGame);
        fn.transform.SetParent(WindowManager.Instance.CurrentWindow.TopPanel.transform);
        fn.Init(tr, str, DataBaseController.Instance.DataStructs.MonsterHeathColor);
        return fn;
    }

    public void Init(Transform dependence, string msg, Color textColor, FlyNumerDirection flyDir = FlyNumerDirection.side,int size = 42)
    {
        base.Init();
        cam = MainController.Instance.MainCamera;
        this.dependence = dependence;
        incFlyingNumbers.Init(msg, textColor, flyDir, size, OnDead);
    }

    private void OnDead()
    {
        EndUse();
    }

    void Update()
    {
        if (IsUsing && dependence != null)
            transform.position = cam.WorldToScreenPoint(dependence.position);
    }
}

