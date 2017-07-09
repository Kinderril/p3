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

    private const int maxAngle = 45 + 90;
    private const int ANG_TO_BACK = 110;
    private const float CPNST_BACK_WALK = 0.52f;
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

    public override bool MoveTo(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            var isWalk = SpinTransform.IsWaiting || SpinTransform.IsRotating;

            if (isWalk)
            {
                dir = dir*CPNST_BACK_WALK;
            }
            else
            {
                SetToDirection(dir );
            }

            m_Rigidbody.velocity = dir;
            float speed = dir.sqrMagnitude;
            if (speed < WALK)
            {
                SetAnimType(TriggerType.idle);
            }
            else if (isWalk)
            {

//                                Vector3 dirFromSpinTo = SpinTransform.qTo * Vector3.forward;
//                                var getSide = Mathf.Sign(dir.x*dirFromSpinTo.z + dir.z*dirFromSpinTo.x) > 0;
//                                Debug.Log("Side:" + getSide + "   dir:" + dir  +  "   contest:"+ dirFromSpinTo + "    SpinTransform.qTo:" + SpinTransform.qTo);
//                var getSide = SpinTransform.qTo.y > 0;
//                Debug.Log(" getSide:"  + getSide);



                var ang = Quaternion.Angle(Quaternion.LookRotation(dir), SpinTransform.qTo);
                if (ang > maxAngle)
                {
                    SetAnimType(TriggerType.back);
                    var v1 = QueaternionFromTo.RotateVectorByY(dir, 180);
                    SetToDirection(v1);
                }
                else if (ang > 45)
                {
                    Vector3 dirFromSpinTo = SpinTransform.qTo * Vector3.forward;
                    var getSide = Mathf.Sign(dir.x * dirFromSpinTo.z + dir.z * dirFromSpinTo.x) > 0;
                    if (getSide)
                    {
                        SetAnimType(TriggerType.left);
//                        var v1 = QueaternionFromTo.RotateVectorByY(dir, 0);
                        SetToDirection(dirFromSpinTo);
                    }
                    else
                    {
                        SetAnimType(TriggerType.right);
//                        var v1 = QueaternionFromTo.RotateVectorByY(dir, -0);
                        SetToDirection(dirFromSpinTo);
                    }
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
            m_Rigidbody.velocity = dir;
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

            var key = GetKeyByType(type);
            Animator.SetBool(key,false);
            curTriggerType = type;
            key = GetKeyByType(curTriggerType);
            Animator.SetBool(key,true);
            
        }
    }

    private string GetKeyByType(TriggerType t)
    {

        switch (curTriggerType)
        {
            case TriggerType.left:
                return (ANIM_LEFT);
            case TriggerType.right:
                return (ANIM_RIGHT);
                break;
            case TriggerType.back:
                return  ANIM_BACK;
                break;
            case TriggerType.forward:
                return (ANIM_FORWARD);
                break;
            case TriggerType.run:
                return (ANIM_RUN);
                break;
            case TriggerType.idle:
                return (ANIM_IDLE);
                break;
        }
        return "";
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
        var doRotate = SpinTransform.SetLookDir(dir);
        return doRotate;
    }

//    protected override void UpdateCharacter()
//    {
//        base.UpdateCharacter();
//        SpinTransform.UpdateRotate();
//        CheckRemainBackDir();
//        if (DebuGameObject != null)
//        {
////            DebuGameObject.SetActive(isBackDir);
//        }
//    }
}

