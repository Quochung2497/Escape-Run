using UnityEngine;

namespace Watermelon
{
    public static class TextureExtensions
    {
        public static void DrawLine(this Texture2D texture, Vector2 p1, Vector2 p2, Color col)
        {
            Vector2 t = p1;
            float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
            float ctr = 0;

            while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
            {
                t = Vector2.Lerp(p1, p2, ctr);
                ctr += frac;
                texture.SetPixel((int)t.x, (int)t.y, col);
            }
        }

        public static void DrawBigDot(this Texture2D texture, int x, int y, int radius, Color color)
        {
            int halfRadius = radius / 2;
            for (int i = x - halfRadius; i < x + halfRadius; i++)
            {
                for (int j = y - halfRadius; j < y + halfRadius; j++)
                {
                    if (i >= 0 && j >= 0 && i < texture.width && j < texture.height)
                    {
                        texture.SetPixel(i, j, color);
                    }
                }
            }
        }
    }
}