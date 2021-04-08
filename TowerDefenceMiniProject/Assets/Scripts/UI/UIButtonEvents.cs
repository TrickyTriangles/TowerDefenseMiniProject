using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIButtonEvents : MonoBehaviour, IPointerEnterHandler
{
    private Action<UIButtonEvents> OnHoverEvent;

    public void SubscribeToMouseOverEvent(Action<UIButtonEvents> listener)
    {
        OnHoverEvent += listener;
    }

    public void UnsubscribeToMouseOverEvent(Action<UIButtonEvents> listener)
    {
        OnHoverEvent -= listener;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEvent?.Invoke(this);
    }
}
