using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System;
/// <summary>
/// ugui事件类
/// </summary>
public class UGUIEventListener : EventTrigger
{
    public Action<GameObject, BaseEventData> OnSubmitEvent;
    public Action<GameObject,PointerEventData> OnClickEvent;

    public Action<GameObject,PointerEventData> OnPointerEnterEvent;
    public Action<GameObject,PointerEventData> OnPointerExitEvent;

    public Action<GameObject, PointerEventData> OnPointerDownEvent;
    public Action<GameObject, PointerEventData> OnPointerUpEvent;


    public Action<PointerEventData> OnBeginDragEvent ;
    public Action<PointerEventData> OnDragEvent;
    public Action<PointerEventData> OnEndDragEvent;


    public override void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpEvent?.Invoke(gameObject, eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownEvent?.Invoke(gameObject, eventData);
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        if (OnSubmitEvent != null)
            OnSubmitEvent(gameObject,eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke(gameObject, eventData);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        OnClickEvent?.Invoke(gameObject, eventData);

    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke(gameObject, eventData);
    }
    
    public override void OnBeginDrag(PointerEventData data)
    {
        if(OnBeginDragEvent!=null)
        OnBeginDragEvent(data);
    }
    public override void OnEndDrag(PointerEventData data)
    {
        if (OnEndDragEvent != null)
            OnEndDragEvent(data);
    }
    public override void OnDrag(PointerEventData data)
    {
        if(OnDragEvent != null)
        OnDragEvent(data);
    }
    public static UGUIEventListener Get(GameObject go)
    {
        UGUIEventListener listener = go.GetComponent<UGUIEventListener>();
        if (listener == null) listener = go.AddComponent<UGUIEventListener>();
        return listener;
    }

}
/*
 using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
 
public class UGUIEventListener:EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public delegate void BoolDelegate(GameObject go, bool isValue);
    public delegate void FloatDelegate(GameObject go, float fValue);
    public delegate void IntDelegate(GameObject go, int iIndex);
    public delegate void StringDelegate(GameObject go, string strValue);
 
    public VoidDelegate onSubmit;
    public VoidDelegate onClick;
    public BoolDelegate onHover;
    public BoolDelegate onToggleChanged;
    public FloatDelegate onSliderChanged;
    public FloatDelegate onScrollbarChanged;
    public IntDelegate onDrapDownChanged;
    public StringDelegate onInputFieldChanged;
 
    public override void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null)
            onSubmit(gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onHover != null)
            onHover(gameObject, true);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(gameObject);
        if (onToggleChanged != null)
            onToggleChanged(gameObject, gameObject.GetComponent<Toggle>().isOn);
 
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onHover != null)
            onHover(gameObject, false);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (onSliderChanged != null)
            onSliderChanged(gameObject, gameObject.GetComponent<Slider>().value);
        if (onScrollbarChanged != null)
            onScrollbarChanged(gameObject, gameObject.GetComponent<Scrollbar>().value);
 
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onDrapDownChanged != null)
            onDrapDownChanged(gameObject, gameObject.GetComponent<Dropdown>().value);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onInputFieldChanged != null)
            onInputFieldChanged(gameObject, gameObject.GetComponent<InputField>().text);
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        if (onInputFieldChanged != null)
            onInputFieldChanged(gameObject, gameObject.GetComponent<InputField>().text);
    }
 
    public static UGUIEventListener Get(GameObject go)
    {
        UGUIEventListener listener =go.GetComponent<UGUIEventListener>();
        if(listener==null) listener=go.AddComponent<UGUIEventListener>();
        return listener;
    }
     */
