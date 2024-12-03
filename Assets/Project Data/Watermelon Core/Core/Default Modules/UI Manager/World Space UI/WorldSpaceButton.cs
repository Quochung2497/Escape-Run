using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Watermelon
{
    [RequireComponent(typeof(Button), typeof(BoxCollider))]
    public class WorldSpaceButton : MonoBehaviour
    {
        private Button button;
        private BoxCollider boxCollider;

        private void Awake()
        {
            button = GetComponent<Button>();
            boxCollider = GetComponent<BoxCollider>();
        }

        public bool Raycast(Ray ray)
        {
            return boxCollider.Raycast(ray, out _, 100);
        }

        public void Press(PointerEventData eventData)
        {
            button.OnPointerDown(eventData);
        }

        public void Release(PointerEventData eventData)
        {
            button.OnPointerUp(eventData);
            button.OnPointerClick(eventData);
        }

        private void OnEnable()
        {
            WorldSpaceRaycaster.AddWorldSpaceButton(this);
        }

        public void OnDisable()
        {
            WorldSpaceRaycaster.RemoveWorldSpaceButton(this);
        }

        private void OnValidate()
        {
            button = GetComponent<Button>();
            boxCollider = GetComponent<BoxCollider>();

            boxCollider.size = ((Vector3)button.image.rectTransform.sizeDelta).SetZ(1);
            boxCollider.isTrigger = true;
        }
    }
}