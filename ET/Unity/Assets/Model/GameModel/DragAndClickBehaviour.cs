using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndClickBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private void Awake()
    {

    }

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnDestroy()
    {
        dragFlag = false;
        rotateAction = null;
        clickAction = null;
    }
    
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        dragFlag = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (rotateAction != null)
        {
            var angel = Mathf.Pow(Mathf.Min(Mathf.Abs(eventData.delta.x) / 25.0f, 1.0f), 2.0f) * 10.0f * Mathf.Sign(eventData.delta.x);
            rotateAction.Invoke(angel);
        }
    }
    
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        dragFlag = false;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (dragFlag)
            return;
        clickAction?.Invoke();
    }
    

    private bool dragFlag = false;
    private Action<float> rotateAction = null;
    private Action clickAction = null;

    public void RegisterEventRotate(Action<float> action)
    {
        rotateAction += action;
    }

    public void RemoveEventRotate(Action<float> action)
    {
        rotateAction = null;
       // rotateAction -= action;
    }

    public void RegisterEventClick(Action action)
    {
        clickAction += action;
    }

    public void RemoveEventClick(Action action)
    {
        clickAction = null;
        // clickAction -= action;
    }
}