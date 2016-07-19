using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMain : MonoBehaviour//,IPointerDownHandler,IPointerUpHandler
{
    private Camera MainCamera;
    private int layerMask = 1;
    private Hero mainHero;
    //private Vector2 startDrag;
    public SubUIMain subUI;
    private bool enable;
    public Vector3 keybordDir;
    

    private float chargeTime;
    private Vector2 startDrag;
    private bool isLastFramePressed;
    private bool isOverUI;
    private bool isCharging = false;
    public Slider chargeSlider;
    private Vector2 dir;
    private int framesPressed = 0;
    private int controlMoreX;

    public Text debugText;
    public Text debugText2;
    public Text debugText3;

    public void Init(Level lvl)
    {
        controlMoreX = Screen.width/2; 
        mainHero = lvl.MainHero;
        MainCamera = MainController.Instance.MainCamera;
        chargeSlider.gameObject.SetActive(false);
        if (subUI != null)
        {
            subUI.Init(this);
        }
        else
        {
            Debug.LogWarning("no sub UI");
        }
    }

    public void OnChangeWeapon()
    {
        mainHero.SwitchWeapon();
    }

    //    public void OnCrouch()
    //    {
    //        if (enable)
    //            mainHero.DoCrouch();
    //    }

    void Update()
    {
#if UNITY_EDITOR
        var w = Input.GetKey(KeyCode.W);
        var s = Input.GetKey(KeyCode.S);
        var d = Input.GetKey(KeyCode.D);
        var a = Input.GetKey(KeyCode.A);
        int x = 0;
        int y = 0;
        if (w)
        {
            x = 1;
        }
        else if (s)
        {
            x = -1;
        }

        if (d)
        {
            y = 1;
        }
        else if (a)
        {
            y = -1;
        }
        keybordDir = new Vector3(y, 0, x);
        if (mainHero != null)
            mainHero.MoveToDirection(keybordDir);
#endif
    }

    private Vector3 RayCast(PointerEventData eventData)
    {
        RaycastHit hit;
        Ray ray = MainCamera.ScreenPointToRay(eventData.position);//Input.mousePosition);
#if UNITY_EDITOR
        Debug.DrawRay(ray.origin, ray.direction * 11, Color.yellow, 1);
#endif
        if (Physics.Raycast(ray, out hit, 9999999, layerMask))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    void UpdateSwipe()
    {
#if UNITY_EDITOR
        var mp = Input.mousePosition;
        if (mp.x <= controlMoreX)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startDrag = mp;
        }

        if (Input.GetMouseButton(0))
        {
            isOverUI = EventSystem.current.IsPointerOverGameObject();
            isLastFramePressed = true;
            isCharging = false;
            chargeTime = Time.time + Weapon.CHARGE_TIME_DELAY;
            dir = new Vector2(mp.x, mp.y) - startDrag;

            //            Debug.Log("dir " + dir);
            if (!isCharging && Time.time > chargeTime)
            {
                var dist = dir.sqrMagnitude;
                if (dist < 4000)
                    StartCharge();
            }

            if (isCharging)
            {
                var perc = (Time.time - chargeTime) / Weapon.MAX_CHARGE_TIME;
                chargeSlider.value = perc;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isLastFramePressed = false;
            if (isOverUI)
            {
                EndPress();
                return;
            }
            if (enable)
            {
                framesPressed = 3;
                EndPress();
            }
        }
#else
//        bool pressedCur = false;
        Touch? touch = null;
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touchTmp = Input.GetTouch(i);
            
            isOverUI = EventSystem.current.IsPointerOverGameObject(touchTmp.fingerId);
            if (!isOverUI && touchTmp.position.x >= controlMoreX)
            {
//                pressedCur = true;
                touch = touchTmp;
                break;
            }
        }
//        if (!touch.HasValue)
//        {
//            pressedCur = false;
//        }

//        debugText.text = "touch.position.x: " + touch.Value.position.x + "  controlMoreX:"+ controlMoreX +  
//            "   Scrren:" +Screen.width; 
//        if (touch == default(Touch) ){
//            return;
//        }
//        debugText2.text = " isLastFramePressed:" + isLastFramePressed + "  pressedCur:" + pressedCur
//                          + "  framesPressed:" + framesPressed;
            
        if (isLastFramePressed)
        {
            if (touch.HasValue)
            {
                ContiniusPress(touch.Value);
            }
            else
            {
                EndPress();
            }
        }
        else
        {
            if (touch.HasValue)
            {
                StartPress(touch.Value);
            }
        }
        isLastFramePressed = touch.HasValue;
#endif
    }
    void LateUpdate()
    {
        if (enable)
        {
            UpdateSwipe();
        }
    }

    private void StartPress(Touch touch)
    {
        startDrag = touch.position;
        chargeTime = Time.time + Weapon.CHARGE_TIME_DELAY;
        framesPressed = 0;
    }

    private void EndPress()
    {
        EndChargeUI();
        if (framesPressed > 2 && mainHero != null && !mainHero.IsDead)
        {
            float chargePower = Time.time - chargeTime;
            var v = new Vector3(dir.x, 0, dir.y);
            mainHero.TryAttackByDirection(v, chargePower, true);
        }
    }

    private void ContiniusPress(Touch touch)
    {
        dir = touch.position - startDrag;
        if (!isCharging && Time.time > chargeTime)
        {
            var dist = dir.sqrMagnitude;
            if (dist < 4000)
                StartCharge();
        }
        framesPressed++;
        var perc = (Time.time - chargeTime) / Weapon.MAX_CHARGE_TIME;
        chargeSlider.value = perc;
    }

    private void EndChargeUI()
    {
        isCharging = false;
        chargeSlider.gameObject.SetActive(isCharging);

    }

    private void StartCharge()
    {
        isCharging = true;
        chargeSlider.gameObject.SetActive(isCharging);

    }

    public void UpdateMoveArrow(Vector3 dir)
    {
        if (enable)
            mainHero.MoveToDirection(dir);
        //        debugText.text = "vel:" + mainHero.Control.Velocity + "  dir:" + dir;

    }

    public void Enable(bool val)
    {
        subUI.Enable(val);
        enable = val;
    }
}

