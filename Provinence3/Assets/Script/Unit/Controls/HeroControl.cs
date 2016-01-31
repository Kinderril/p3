using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Effects;

public class HeroControl : BaseControl
{
    private const int ANG_TO_BACK = 110;
    private const float CPNST_BACK_WALK = 0.62f;
    private const float CONST_SEC_WALK = 1.4f;
    private const float CONST_SEC_LOOK = 1.4f;
    private float RemainBackWalkTimeSec = 0;
    private float TimeToGoToDefaultLook;
    private bool useLookDir = false;
    private Vector3 lookDir;
    private Vector3 lastMoveDir;
    public bool isBackDir;
    public QueaternionFromTo SpinTransform;
    public GameObject DebuGameObject;
    
    public void Init(Action comeToRotation)
    {
        SpinTransform.Init(OnLookEnd, comeToRotation);
    }

    private void OnLookEnd()
    {
//        Debug.Log("OnLookEnd");
//        SpinTransform.SetLookDir(targetDirection);
//        Animator.SetBool(ANIM_ATTACK, false);
    }

    public override bool MoveTo(Vector3 v)
    {
        if (v != Vector3.zero)
        {
            if (SpinTransform.IsWaiting || SpinTransform.IsRotating)
            {
                var ang = Quaternion.Angle(Quaternion.LookRotation(v), SpinTransform.qTo);
                if (ang > ANG_TO_BACK)
                {

                    SetBackDir(true);
                }
                else
                {
                    SetBackDir(false);
                }
            }
            else
            {
                SetBackDir(false);
            }
            if (isBackDir)
            {
                v = v*CPNST_BACK_WALK;
                SetToDirection(-v);
            }
            else
            {

                SetToDirection(v);
            }

            /*
            if (IsMoving() && SpinTransform.IsWaiting)
            {
                if (ang > 110)
                {
                    SetBackDir(true);
                }
                if (isBackDir)
                {
                    v = v * CPNST_BACK_WALK;
                    SetToDirection(-v);
                }
                else
                {
                    SetToDirection(v);
                }
            }
            else  if (isBackDir)
            {
                if (ang < 110)
                {
                    SetBackDir(false,"diffff a");
                }
//                Debug.Log("dir back1 " + (-v));
                v = v * CPNST_BACK_WALK;
                SetToDirection(-v);
            }
            else
            {
                SetToDirection(v);
            }*/
        }
        m_Rigidbody.velocity = v;
        UpdateAnimator(v);
        return true;
    }

    public override void SetToDirection(Vector3 dir)
    {
        if (lastMoveDir != dir)
        {
            lastMoveDir = dir;
            base.SetToDirection(dir);
        }
    }

    private void SetBackDir(bool value,string cause = "")
    {
        //CHeck on look. maybe we wait here
        //        Debug.Log("SetBackDir " + value);
        //        if (value == isBackDir)
        //
        //        if (SpinTransform.IsWaiting)
        //        {
        //            
        //        }
        isBackDir = value;

//        if (value)
//        {
//            RemainBackWalkTimeSec = CONST_SEC_WALK;
//        }
//        if (IsMoving())
//        {
//            isBackDir = value;
//        }
//        else
//        {
//            isBackDir = false;
//        }
//        Debug.Log("Set backl dir " + value + "   " + Time.time + "   cause:" + cause);
    }

    private bool IsMoving()
    {
        return m_Rigidbody.velocity.sqrMagnitude > 0.1f;
    }
    
    public bool SetLookDir(Vector3 dir)
    {
        var angel = Quaternion.Angle(Quaternion.LookRotation(dir), SpinTransform.qTo);
        var isMoving = IsMoving();
        if (isMoving)
        {
            if (angel > ANG_TO_BACK)
            {
                SetBackDir(true);
            }
            else
            {
                SetBackDir(false);
            }
        }
        else
        {
            SetToDirection(dir);
        }
        var doRotate = SpinTransform.SetLookDir(dir);
        return doRotate;
    }

    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
//        SpinTransform.UpdateRotate();
//        CheckRemainBackDir();
        if (DebuGameObject != null)
        {
            DebuGameObject.SetActive(isBackDir);
        }
    }
}

