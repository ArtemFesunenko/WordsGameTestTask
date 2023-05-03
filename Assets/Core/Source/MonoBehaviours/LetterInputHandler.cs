using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LetterInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public event Action OnPointerDownEvent;
    public event Action OnPointerUpEvent;
    public event Action OnPointerEnterEvent;

    [SerializeField] private Image image;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownEvent?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpEvent?.Invoke();
    }

    public void SetActive(bool active)
    {
        image.color = active ? Color.green : Color.white;
    }

    public void ClearEvents()
    {
        OnPointerDownEvent = null;
        OnPointerUpEvent = null;
        OnPointerEnterEvent = null;
    }
}
