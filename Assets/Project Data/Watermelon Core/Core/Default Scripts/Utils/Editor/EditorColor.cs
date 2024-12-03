using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    public static class EditorColor
    {
        //Special
        public static readonly Color jellyBean = new Color32(222, 107, 72, 255);
        public static readonly Color straw = new Color32(225, 206, 122, 255);
        public static readonly Color lightApricot = new Color32(252, 215, 173, 255);
        public static readonly Color englishRed = new Color32(165, 70, 87, 255);
        public static readonly Color greenPantone = new Color32(8, 160, 69, 255);

        //Cyan
        public static readonly Color cyan01 = new Color32(4, 133, 157, 255);
        public static readonly Color cyan02 = new Color32(32, 104, 118, 255);
        public static readonly Color cyan03 = new Color32(1, 86, 102, 255);
        public static readonly Color cyan04 = new Color32(55, 182, 206, 255);
        public static readonly Color cyan05 = new Color32(95, 189, 206, 255);

        //Blue
        public static readonly Color blue01 = new Color32(20, 55, 173, 255);
        public static readonly Color blue02 = new Color32(44, 64, 129, 255);
        public static readonly Color blue03 = new Color32(6, 31, 112, 255);
        public static readonly Color blue04 = new Color32(72, 105, 214, 255);
        public static readonly Color blue05 = new Color32(110, 134, 214, 255);

        //Green
        public static readonly Color green01 = new Color32(0, 193, 43, 255);
        public static readonly Color green02 = new Color32(36, 145, 60, 255);
        public static readonly Color green03 = new Color32(156, 255, 147, 255);
        public static readonly Color green04 = new Color32(56, 224, 93, 255);
        public static readonly Color green05 = new Color32(101, 224, 128, 255);

        //Green
        public static readonly Color orange01 = new Color32(255, 124, 0, 255);
        public static readonly Color orange02 = new Color32(191, 118, 48, 255);
        public static readonly Color orange03 = new Color32(166, 81, 0, 255);
        public static readonly Color orange04 = new Color32(255, 157, 64, 255);
        public static readonly Color orange05 = new Color32(255, 183, 115, 255);

        //Red
        public static readonly Color red01 = new Color32(219, 84, 97, 255);
        public static readonly Color red02 = new Color32(190, 56, 62, 255);

        public static readonly Color editorLightColor = new Color32(194, 194, 194, 255);
        public static readonly Color editorBlackColor = new Color32(56, 56, 56, 255);

        public static Color EditorDefaultColor
        {
            get
            {
                return EditorGUIUtility.isProSkin ? editorBlackColor : editorLightColor;
            }
        }
    }
}