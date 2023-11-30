using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeField : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public event Action<float> OnHorizntalSwiping;


    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.delta.x != 0)
        {
            OnHorizntalSwiping?.Invoke(eventData.delta.x);
        }
    }
}