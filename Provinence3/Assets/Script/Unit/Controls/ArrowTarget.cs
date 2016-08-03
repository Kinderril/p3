using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ArrowTarget : MonoBehaviour
{
    private const float sec_time_out_sec = 0.7f;
    private BossUnit BossUnit;
    public int distToDisappear;
    private bool isActive;
    public QueaternionFromTo rotateObject;
    private float lastRotateTime = 0;
    public GameObject ArrowGameObject;

    void Awake()
    {
        ArrowGameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    

    public void Init(BossUnit boss)
    {
        BossUnit = boss;
        BossUnit.Arrow = this; 
//        MainController.Instance.TimerManager 
        ArrowGameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void UpdateByBoss()
    {
        if (isActive && (Time.time - lastRotateTime) > sec_time_out_sec)
        {
            lastRotateTime = Time.time;
            var dir = BossUnit.transform.position - transform.position;
            if (!rotateObject.IslookingSame(dir))
            {
                rotateObject.SetLookDir(dir);
            }
        }
        isActive = BossUnit.mainHeroDist > distToDisappear;
        ArrowGameObject.SetActive(isActive);
    }
}

