using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class QueaternionFromTo : MonoBehaviour
{
    //    public Vector3 to;
    //    public Vector3 from;
    public Quaternion qTo;
    public Quaternion qFrom;
    private Quaternion qCur;
    private float time;
    public float totalSpeed;
    private float curSpeed;
    private float ang;
    private float remainAngel;
    public bool isRotating;
    public QueaternionFromTo BlockingFromTo;
    public float waitTimeSec;
    private Action endLookRotation;
    private Action comeToRotation;
    public bool isWaiting;
    public float endWaitTime;

    public bool IsRotating
    {
        get { return isRotating; }
    }
    public bool IsWaiting
    {
        get { return isWaiting; }
    }

    public float RemainAngel
    {
        get { return remainAngel; }
    }

    private void Start()
    {

    }

    public bool SetLookDir(Vector3 dir)
    {
        qFrom = transform.rotation;
        qTo = Quaternion.LookRotation(dir);
        ang = Quaternion.Angle(qFrom, qTo);
        time = 0;
        if (ang > 2)
        {
            curSpeed = totalSpeed /ang;
            
            isRotating = true;
            isWaiting = false;
        }
//        Debug.Log("ang:"+ ang);
        return IsRotating;
    }

    private void LateUpdate()
    {
        if (isRotating)
        {
            time += Time.deltaTime * curSpeed;
            if (NoBlocking())
            {
                if (time >= 1f)
                {
                    time = 1f;
//                    Debug.Log("ENd tor " + curSpeed);
                    comeToRotation();
                    isRotating = false;
                    StartWait();
                }
                remainAngel = (1f - time)*ang;
                qCur = Quaternion.Lerp(qFrom, qTo, time);
                transform.rotation = qCur;
            }
        }
        else if (isWaiting)
        {
            if (NoBlocking())
            {
                transform.rotation = qCur;
                if (Time.time > endWaitTime)
                {
                    isWaiting = false;
                    if (endLookRotation != null)
                    {
                        endLookRotation();
                    }
                }
            }
            else
            {
//                Debug.Log("By block: " + BlockingFromTo.RemainAngel + "  " +BlockingFromTo.qFrom.eulerAngles + "  " + BlockingFromTo.qTo.eulerAngles + "  " + BlockingFromTo.qCur.eulerAngles);
                
                isWaiting = false;
            }
        }
    }

    private bool NoBlocking()
    {
//        if (BlockingFromTo == null)
//            return true;
//
//        if (!BlockingFromTo.IsRotating)
//            return true;
//
//        if (Quaternion.Angle(BlockingFromTo.qTo, qTo) < 90)
//            return true;
//
//        return false;
        return true;
//        return !(BlockingFromTo != null && BlockingFromTo.IsRotating && BlockingFromTo.RemainAngel > 45);
    }

    private void StartWait()
    {
        if (waitTimeSec > 0)
        {
            isWaiting = true;
            endWaitTime = waitTimeSec + Time.time;
        }
    }

    public void Init(Action onLookEnd, Action onComeRotation)
    {
        this.comeToRotation = onComeRotation;
        this.endLookRotation = onLookEnd;
        if (BlockingFromTo != null)
        {
            totalSpeed = BlockingFromTo.totalSpeed*1.3f;
        }
    }

    public bool ShallRotate(Vector3 dir)
    {
        return Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir)) < 4;
    }

}