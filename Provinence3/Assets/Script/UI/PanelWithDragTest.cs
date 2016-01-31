using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PanelWithDragTest : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{

    public RectTransform show;
    private Vector2 startPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        show.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        show.transform.position = show.transform.position + new Vector3(eventData.delta.x, eventData.delta.y);
    }
}
