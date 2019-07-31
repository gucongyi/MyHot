using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    const float interval=0.1f;
    public UnityEvent onLongPress = new UnityEvent();//长按时调用
    public UnityEvent onLongPressRelease = new UnityEvent();//松开时调用
    Button button;
    bool isPointerDown = false;
    float recordTime;
    bool hadInvoke=false;
    // Use this for initialization
    void Start()
    {
        button = GetComponent<Button>();
        if (button == null) Debug.LogError("没有Button组件");
    }

    // Update is called once per frame
    void Update()
    {
        if (button == null) return;
        if (button.IsInteractable() == false) return;
        if (hadInvoke) return;
        if (isPointerDown)
        {
            if ((Time.time - recordTime) > interval)
            {
                onLongPress?.Invoke();
                hadInvoke = true;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        recordTime = Time.time;
        isPointerDown = true;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        hadInvoke = false;
        onLongPressRelease?.Invoke();
    }
}
