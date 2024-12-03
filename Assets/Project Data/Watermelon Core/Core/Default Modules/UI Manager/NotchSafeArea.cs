using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class NotchSaveArea
    {
        [SerializeField] RectTransform[] safePanels;

        [Space]
        [SerializeField] bool ConformX = true;
        [SerializeField] bool ConformY = true;

        private Rect lastSafeArea = new Rect(0, 0, 0, 0);
        private Vector2Int lastScreenSize = new Vector2Int(0, 0);

        private ScreenOrientation lastOrientation = ScreenOrientation.AutoRotation;

        public void Refresh()
        {
            Rect safeArea = Screen.safeArea;

            if (safeArea != lastSafeArea || Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y || Screen.orientation != lastOrientation)
            {
                lastScreenSize.x = Screen.width;
                lastScreenSize.y = Screen.height;
                lastOrientation = Screen.orientation;

                ApplySafeArea(safeArea);
            }
        }

        private void ApplySafeArea(Rect rect)
        {
            lastSafeArea = rect;

            // Ignore x-axis?
            if (!ConformX)
            {
                rect.x = 0;
                rect.width = Screen.width;
            }

            // Ignore y-axis?
            if (!ConformY)
            {
                rect.y = 0;
                rect.height = Screen.height;
            }

            // Check for invalid screen startup state on some Samsung devices (see below)
            if (Screen.width > 0 && Screen.height > 0)
            {
                // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
                Vector2 anchorMin = rect.position;
                Vector2 anchorMax = rect.position + rect.size;

                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                // Fix for some Samsung devices (e.g. Note 10+, A71, S20) where Refresh gets called twice and the first time returns NaN anchor coordinates
                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    for (int i = 0; i < safePanels.Length; i++)
                    {
                        safePanels[i].anchorMax = anchorMax;
                    }
                }
            }
        }
    }
}
