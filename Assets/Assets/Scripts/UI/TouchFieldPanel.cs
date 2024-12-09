using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class TouchFieldPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static TouchFieldPanel instance { get; private set; }

    [Header("Joystick Settings")]
    [SerializeField] private RectTransform joystickRectTransform;
    public GameObject joystickGameObject;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private FixedJoystick fixedJoystick;

    [Header("Events")]
    public UnityEvent OnTap;

    //private
    private bool isPressed;
    private bool isStaminaPhaseComplete = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isStaminaPhaseComplete)
        {
            OnTap?.Invoke();
        }
        else
        {
            ActivateJoystick(eventData);
        }

        isPressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        if (isStaminaPhaseComplete)
        {
            DeactivateJoystick(eventData);
        }
    }
    private void ActivateJoystick(PointerEventData eventData)
    {
        if (!joystickGameObject.activeSelf)
        {
            joystickGameObject.SetActive(true);
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, null, out localPoint);
            joystickRectTransform.anchoredPosition = localPoint;
            fixedJoystick.OnPointerDown(eventData);
        }
    }
    private void DeactivateJoystick(PointerEventData eventData)
    {
        if (joystickGameObject.activeSelf)
        {
            joystickGameObject.SetActive(false);
            fixedJoystick.OnPointerUp(eventData);
        }
    }
    public void CompleteStaminaPhase()
    {
        isStaminaPhaseComplete = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (joystickGameObject.activeSelf)
        {
            fixedJoystick.OnDrag(eventData);
        }
    }

    public bool joystickActive()
    {
        return joystickGameObject.activeInHierarchy;
    }
}
