using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchFieldPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Events")]
    public UnityEvent OnTap;
    private bool isPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        OnTap?.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
