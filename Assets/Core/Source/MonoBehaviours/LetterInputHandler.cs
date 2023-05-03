using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LetterInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public event Action OnPointerDownEvent;
    public event Action OnPointerUpEvent;
    public event Action OnPointerEnterEvent;

    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image image;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    private void Start()
    {
        SetActive(false);
    }

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
        image.color = active ? activeColor : inactiveColor;
        textMesh.color = active ? Color.white : Color.black;
    }

    public void ClearEvents()
    {
        OnPointerDownEvent = null;
        OnPointerUpEvent = null;
        OnPointerEnterEvent = null;
    }
}
