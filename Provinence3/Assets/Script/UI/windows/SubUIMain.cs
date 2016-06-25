using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SubUIMain : MonoBehaviour,IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    private Vector2 startPos;
    private Vector2 dir;
    public bool isDrag;
    private UIMain uiMain;
    private bool enable = false;
    public RectTransform Arrow;
    public void Init(UIMain uiMain)
    {
        this.uiMain = uiMain;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (enable)
        {
            startPos = eventData.position;
            Arrow.transform.position = startPos;
            isDrag = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        if (isDrag)
        {
            dir = (eventData.position - startPos).normalized;
            var v = new Vector3(dir.x, 0, dir.y);
            uiMain.UpdateMoveArrow(v.normalized);
            float zRotate = Mathf.Acos(dir.y / (Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y)));
            zRotate *= Mathf.Rad2Deg;
            if (eventData.position.x > startPos.x)
            {
                zRotate = 360 - zRotate;
            }
            Arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, zRotate));
            //Arrow.LookAt(Direction  + startPos);
        }
//        debugDir.text = " isDrag:" + isDrag + "   dir:" + dir;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
        uiMain.UpdateMoveArrow(Vector3.zero);
    }
    public void Enable(bool val)
    {
        enable = val;
    }
}

