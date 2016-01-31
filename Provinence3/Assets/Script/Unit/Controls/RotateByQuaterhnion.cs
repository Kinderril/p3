using System;
using UnityEngine;
using System.Collections;

public class RotateByQuaterhnion : MonoBehaviour
{

    private float yy = 0;
    private const float waitTime = 1.6f;
    private const int border = 360;
    public float speed = 10;
    private float timeToOffWait;
    public int side = 0;
    public Vector3 lookDir;
    public Vector3 lastLookDir;
    public bool shallRotate = false;
    public bool isWaiting = false;
    private float angle;
    private Quaternion lastQuaternion;
    private Action endLookRotation;
    private Action comeToRotation;
    public int offset;
    public bool shallWait = false;
    public bool shallWait2 = true;

    public int Side
    {
        get { return side; }
    }

    public bool IsRotating
    {
        get { return shallRotate; }
    }


    public void Init(Action endLookRotation, Action comeToRotation)
    {
        this.endLookRotation = endLookRotation;
        this.comeToRotation = comeToRotation;
    }

    void LateUpdate()
    {
        UpdateRotate();
    }
    private void UpdateRotate ()
    {
        if (shallRotate)
        {
            angle = Mathf.Abs(lastLookDir.y - lookDir.y);
            if (angle < 4)
            {

                lastLookDir = lookDir;
                lastQuaternion = Quaternion.Euler(lastLookDir);
                transform.rotation = lastQuaternion;
                shallRotate = false;
                comeToRotation();
                if (shallWait && shallWait2)
                {
                    timeToOffWait = Time.time + waitTime;
                    isWaiting = true;
                }
            }
            else
            {

                yy = lastLookDir.y + Time.deltaTime * speed * side;
                yy = FixAngle(yy);

                lastLookDir = new Vector3(0, yy, 0);
                lastQuaternion = Quaternion.Euler(lastLookDir);
                transform.rotation = lastQuaternion;
                angle = Mathf.Abs(lastLookDir.y - lookDir.y);
//                Debug.Log("SetNewDir lookDir:" + lookDir + "   last:" + lastLookDir + "   " + side + "   angle:" + angle);
            }
        }
        else if (isWaiting)
        {
            timeToOffWait -= Time.deltaTime;
            transform.rotation = lastQuaternion;
            if (Time.time > timeToOffWait)
            {
                if (endLookRotation != null)
                {
                    endLookRotation();
                }
                isWaiting = false;
            }
        }

    }

    private float FixAngle(float a)
    {

        if (a > border)
        {
            a -= border;
            a = FixAngle(a);
        }
        else if (a < 0)
        {
            a += border;
            a = FixAngle(a);
        }
        return a;
    }

//    void Update()
//    {
//        transform.rotation = Quaternion.identity;
//    }

    public bool ShallRotate(Vector3 dir)
    {
        return Vector3.Angle(dir, lastLookDir) < 4;
    }

    public bool SetLookDir(Vector3 dir, int side)
    {
        shallWait2 = true;
        Debug.Log("SetLookDir:    " + " " + dir + "    isWait:" + isWaiting);
        var ang = Vector3.Angle(dir, new Vector3(-1, 0, 0));
        if (dir.z < 0)
        {
            ang *= -1;
            ang -= offset;
        }
        else
        {
            ang -= offset;
        }
        //var ang = Vector3.Angle(dir, new Vector3(1, 0, 0));
        ang = FixAngle(ang);
        this.side = side;
        shallRotate = true;
        isWaiting = false;
        lookDir = new Vector3(0, ang, 0);
        lastLookDir = transform.rotation.eulerAngles;
        return true;
    }

    public bool SetLookDir(Vector3 dir,bool shallWait = true)
    {
        shallWait2 = shallWait;
//        Debug.Log("SetLookDir "+ " " + dir);
        var ang = Vector3.Angle(dir, new Vector3(-1, 0, 0)) ;
        if (dir.z < 0)
        {
            ang *= -1;
            ang -= offset;
        }
        else
        {
            ang -= offset;
        }
//        this.shallWait = shallWait;
        //var ang = Vector3.Angle(dir, new Vector3(1, 0, 0));
        ang = FixAngle(ang);

        lookDir = new Vector3(0,ang,0);
        lastLookDir = transform.rotation.eulerAngles;
        
        float c;
        side = CalcSide(lastLookDir, lookDir,out c);
        bool needToRotate = Mathf.Abs(c) < 2;
        if (!needToRotate)
        {
            isWaiting = false;
            shallRotate = true;
        }
        return needToRotate;
    }

    public static int CalcSide(Vector3 from, Vector3 to, out float angel)
    {
        int side;
        var a = to.y;
        var b = from.y;
        float c;
        bool v = a > b;
        if (v)
        {
            c = a - b;
            if (c > 180)
            {
                side = -1;
            }
            else
            {
                side = 1;
            }
        }
        else
        {
            c = b - a;
            if (c < 180)
            {
                side = -1;
            }
            else
            {
                side = 1;
            }

        }
        angel = c;
        return side;
    }
}
