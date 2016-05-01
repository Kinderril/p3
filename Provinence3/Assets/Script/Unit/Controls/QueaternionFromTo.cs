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
    public Transform GetRotationFrom;
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

    private Quaternion GetCurrentRotation()
    {
        if (GetRotationFrom == null)
        {
            return transform.rotation;
        }
        else
        {
            return GetRotationFrom.rotation;
        }
    }


    public static Vector3 RotateVectorByY(Vector3 d, float ang = 30)
    {
        ang = Mathf.Deg2Rad*ang;

        var l1 = new Vector3(Mathf.Cos(ang), 0, Mathf.Sin(ang));
        var l2 = new Vector3(0,1,0);
        var l3 = new Vector3(-Mathf.Sin(ang), 0,Mathf.Cos(ang));

        var a1 = l1.x*d.x + l1.y*d.y + l1.z*d.z;
        var a2 = l2.x * d.x + l2.y * d.y + l2.z * d.z;
        var a3 = l3.x * d.x + l3.y * d.y + l3.z * d.z;
        
        return new Vector3(a1,a2,a3);
    }

    public bool SetLookDir(Vector3 dir)
    {
        qTo = Quaternion.LookRotation(dir);
        qFrom = GetCurrentRotation();
        ang = Quaternion.Angle(qFrom, qTo);
        time = 0;
        if (ang > 2)
        {
            curSpeed = totalSpeed /ang;
            
            isRotating = true;
            isWaiting = false;
        }
//        Debug.Log("ang:"+ ang + "   dir:" + dir);
        return IsRotating;
    }

    private void LateUpdate()
    {
        if (isRotating)
        {
            time += Time.deltaTime * curSpeed;
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
        else if (isWaiting)
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

        return Quaternion.Angle(GetCurrentRotation(), Quaternion.LookRotation(dir)) < 4;
    }

}