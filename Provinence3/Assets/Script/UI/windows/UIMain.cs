using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public  class UIMain : MonoBehaviour//,IPointerDownHandler,IPointerUpHandler
{
    private Camera MainCamera;
    private int layerMask = 1;
    private Hero mainHero;
    //private Vector2 startDrag;
    public SubUIMain subUI;
    private bool enable;
    public Vector3 keybordDir;

    private float chargeTime;
    private Vector3 startDrag;
    private bool isPressed;
    private bool isOverUI;
    private bool isCharging = false;
    public Slider chargeSlider;

    //    public Text debugText;
    //    public Text debugText2;

    public void Init(Level lvl)
    {
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

    public void OnCrouch()
    {
        if (enable)
            mainHero.DoCrouch();
    }

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
        keybordDir = new Vector3(y,0,x);
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

    void LateUpdate()
    {
        int index = 0;
        if (Input.touchCount > 1)
        {
            index = 1;
        }

        if (Input.GetMouseButtonDown(index))
        {
            isOverUI = EventSystem.current.IsPointerOverGameObject();
            isPressed = true;
            isCharging = false;
            startDrag = Input.mousePosition;
            chargeTime = Time.time + Weapon.CHARGE_TIME_DELAY;
//            Debug.Log("charge " + chargeTime);
        }

//        Debug.Log("isPressed " + isPressed);
        if (isPressed)
        {
            var isOverUI2 = EventSystem.current.IsPointerOverGameObject();
            var dir = Input.mousePosition - startDrag;

            if (!isCharging && Time.time > chargeTime)
            {
                var dist = dir.sqrMagnitude;
//                Debug.Log("charge " + chargeTime + "   isCharging:" + isCharging + "  dist:" + dist);
                if (dist < 4000)
                    StartCharge();
            }

            if (isCharging)
            {
                var perc = (Time.time - chargeTime) / Weapon.MAX_CHARGE_TIME;
                chargeSlider.value = perc;
            }
            if (Input.GetMouseButtonUp(index))
            {
                isPressed = false;
                if (isOverUI || isOverUI2)
                {
                    EndCharge();
                    return;
                }
                if (enable)
                {
                    EndCharge();
                    float chargePower = Time.time - chargeTime;
                    var v = new Vector3(dir.x, 0, dir.y);
                    mainHero.TryAttackByDirection(v, chargePower);
                }
            }
        }
    }

    private void EndCharge()
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
        enable = val;
    }
}

