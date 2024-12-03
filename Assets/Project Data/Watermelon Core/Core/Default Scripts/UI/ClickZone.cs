using UnityEngine;
using UnityEngine.EventSystems;

namespace Watermelon
{
    public class ClickZone : MonoBehaviour, IPointerClickHandler
    {
        private static ClickZone instance;

        public ClickCallback onZoneClick;
        public static ClickCallback OnZoneClick
        {
            get { return instance.onZoneClick; }
            set { instance.onZoneClick = value; }
        }

        private bool isOpened;

        private void Awake()
        {
            instance = this;

            gameObject.SetActive(false);
        }

        public static void Close()
        {
            if (instance.isOpened)
            {
                instance.gameObject.SetActive(false);

                OnZoneClick(false);

                instance.isOpened = false;
            }
        }

        public static void Open()
        {
            if (!instance.isOpened)
            {
                instance.gameObject.SetActive(true);

                OnZoneClick(true);

                instance.isOpened = true;
            }
            else
            {
                OnZoneClick(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Close();
        }

        public delegate void ClickCallback(bool state);
    }
}