using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Watermelon
{
    public static class WorldSpaceRaycaster
    {
        private static List<WorldSpaceButton> buttons;

        public static void AddWorldSpaceButton(WorldSpaceButton button)
        {
            if (buttons == null) buttons = new List<WorldSpaceButton>();

            if (!buttons.Contains(button)) buttons.Add(button);
        }

        public static void RemoveWorldSpaceButton(WorldSpaceButton button)
        {
            if (buttons == null) buttons = new List<WorldSpaceButton>();

            buttons.Remove(button);
        }

        private static WorldSpaceButton pressedButton;

        public static bool Raycast(PointerEventData eventData)
        {
            if (buttons.IsNullOrEmpty()) return false;

            var ray = Camera.main.ScreenPointToRay(eventData.position);

            var closestDistance = float.MaxValue;
            pressedButton = null;

            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];

                if (button.Raycast(ray))
                {
                    var distance = Vector3.Distance(button.transform.position, Camera.main.transform.position);

                    if(distance < closestDistance)
                    {
                        closestDistance = distance;
                        pressedButton = button;
                    }                    
                }
            }

            if(pressedButton != null)
            {
                pressedButton.Press(eventData);

                return true;
            }

            return false;
        }

        public static void OnPointerUp(PointerEventData eventData)
        {
            if(pressedButton != null)
            {
                pressedButton.Release(eventData);

                pressedButton = null;
            }
        }
    }
}