using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMono : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Action<PointerEventData> OnBeginDragEvent = x => { };
    public Action<PointerEventData> OnDragEvent = x => { };
    public Action<PointerEventData> OnEndDragEvent = x => { };
    public void OnBeginDrag(PointerEventData data)
    {
        OnBeginDragEvent(data);
    }
    public void OnEndDrag(PointerEventData data)
    {
        OnEndDragEvent(data);
    }
    public void OnDrag(PointerEventData data)
    {
        OnDragEvent(data);
    }
}
