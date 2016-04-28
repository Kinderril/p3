using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using UnityEngine;
using UnityStandardAssets.Effects;

public enum TriggerType
{
    left,
    right,
    back,
    forward,
    run,
    idle,
}

public class HeroControl : BaseControl
{
    private const string ANIM_LEFT = "left";
    private const string ANIM_RUN = "run";
    private const string ANIM_IDLE = "idle";
    private const string ANIM_RIGHT = "right";
    private const string ANIM_BACK = "back";
    private const string ANIM_FORWARD = "forward";

    private const int ANG_TO_BACK = 110;
    private const float CPNST_BACK_WALK = 0.62f;
    private const float CONST_SEC_WALK = 1.4f;
    private const float CONST_SEC_LOOK = 1.4f;
    private float RemainBackWalkTimeSec = 0;
    private float TimeToGoToDefaultLook;
    private bool useLookDir = false;
    private Vector3 lookDir;
    private TriggerType curTriggerType = TriggerType.idle;
    private Vector3 lastMoveDir;
//    public bool isBackDir;
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
            var isWalk = SpinTransform.IsWaiting || SpinTransform.IsRotating;

            if (isWalk)
            {
                v = v*CPNST_BACK_WALK;
            }
            else
            {
                SetToDirection(v );
            }

            m_Rigidbody.velocity = v;
            float speed = v.sqrMagnitude;
            if (speed < WALK)
            {
                SetAnimType(TriggerType.idle);
            }
            else if (isWalk)
            {
                var ang = Quaternion.Angle(Quaternion.LookRotation(v), SpinTransform.qTo);
                if (ang > 45 + 90)
                {
                    SetAnimType(TriggerType.back);
                    SetToDirection(v + new Vector3(0,0,90));
                }
                else if (ang > 45)
                {
                    SetAnimType(TriggerType.left);
                    SetToDirection(v + new Vector3(0, 0, 45));
                }
                else
                {
                    SetAnimType(TriggerType.forward);
                }
            }
            else
            {
                SetAnimType(TriggerType.run);
            }
        }
        else
        {
            m_Rigidbody.velocity = v;
            SetAnimType(TriggerType.idle);
        }
//        Debug.Log("trigger: " + d + "     speed:" + speed);
        //        moving = speed > WALK;
        //        Animator.SetBool(ANIM_WALK, moving);

        return true;
    }

    private void SetAnimType(TriggerType type)
    {
        if (type != curTriggerType)
        {
            curTriggerType = type;
            switch (curTriggerType)
            {
                case TriggerType.left:
                    Animator.SetTrigger(ANIM_LEFT);
                    break;
                case TriggerType.right:
                    Animator.SetTrigger(ANIM_RIGHT);
                    break;
                case TriggerType.back:
                    Animator.SetTrigger(ANIM_BACK);
                    break;
                case TriggerType.forward:
                    Animator.SetTrigger(ANIM_FORWARD);
                    break;
                case TriggerType.run:
                    Animator.SetTrigger(ANIM_RUN);
                    break;
                case TriggerType.idle:
                    Animator.SetTrigger(ANIM_IDLE);
                    break;
            }
        }
    }

    public override void SetToDirection(Vector3 dir)
    {
        if (lastMoveDir != dir)
        {
            lastMoveDir = dir;
            base.SetToDirection(dir);
        }
    }

//    private void SetBackDir(bool value,string cause = "")
//    {
//        isBackDir = value;
//    }

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
//                SetBackDir(true);
            }
            else
            {
//                SetBackDir(false);
            }
        }
        else
        {
            SetToDirection(dir);
        }
//        Debug.Log("loookkkk " + dir);
        var doRotate = SpinTransform.SetLookDir(dir,true);
        return doRotate;
    }

    protected override void UpdateCharacter()
    {
        base.UpdateCharacter();
//        SpinTransform.UpdateRotate();
//        CheckRemainBackDir();
//        if (DebuGameObject != null)
//        {
////            DebuGameObject.SetActive(isBackDir);
//        }
    }
}

