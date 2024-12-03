using UnityEngine;

namespace Watermelon
{
    public static class CurrencyUIHelper
    {
        public const float PANEL_HEIGHT = 115.0f;

        private static readonly Vector2[] PANEL_SIZES = new Vector2[]
        {
            new Vector2(200.0f, PANEL_HEIGHT),
            new Vector2(215.0f, PANEL_HEIGHT),
            new Vector2(240.0f, PANEL_HEIGHT),
            new Vector2(268.0f, PANEL_HEIGHT),
            new Vector2(285.0f, PANEL_HEIGHT),
            new Vector2(300.0f, PANEL_HEIGHT)
        };

        public static Vector2 GetPanelSize(int charactersCount)
        {
            if (PANEL_SIZES.IsInRange(charactersCount))
                return PANEL_SIZES[charactersCount];

            return PANEL_SIZES[PANEL_SIZES.Length - 1];
        }
    }
}