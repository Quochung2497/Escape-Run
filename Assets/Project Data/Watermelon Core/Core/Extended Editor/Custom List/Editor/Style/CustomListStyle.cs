using System;
using UnityEditor;
using UnityEngine;

namespace Watermelon.List
{
    [Serializable]
    public class CustomListStyle
    {
        [SerializeField] public string name;
        public Header header;
        public ElementList list;

        //element
        public Element element;
        public DragHandle dragHandle;
        public Foldout foldout;
        public RemoveElementButton removeElementButton;

        public Pagination pagination;
        public FooterButtons footerButtons;
        public BackgroundConfiguration globalBackground;

        //enable
        public bool enableHeader;
        public bool enableElementRemoveButton;
        public bool enableFooterRemoveButton;
        public bool enableFooterAddButton;

        //unsorted mess
        #region Reordable list(original) default values
        private const string DRAGGING_HANDLE_STYLE_NAME = "RL DragHandle";
        private const string HEADER_STYLE_NAME = "RL Header";
        private const string EMPTY_HEADER_STYLE_NAME = "RL Empty Header";
        private const string FOOTER_STYLE_NAME = "RL Footer";
        private const string BACKGROUND_STYLE_NAME = "RL Background";
        private const string FOOTER_BUTTON_STYLE_NAME = "RL FooterButton";
        private const string ELEMENT_STYLE_NAME = "RL Element";
        private const string FOLDOUT = "Foldout";


        public string EMPTY_LIST_LABEL = "List is empty";
        #endregion


        [NonSerialized] public string SEPARATOR = " / ";

        public CustomListStyle()
        {
        }

        public void SetDefaultStyleValues()
        {
            name = "Default style";
            header = new Header();
            header.height = 20;
            header.labelStyle = new GUIStyle(GUI.skin.label);
            header.labelStyle.alignment = TextAnchor.MiddleCenter;
            header.backgroundConfiguration = new BackgroundConfiguration();
            header.backgroundConfiguration.rects = new ColorRect[1];
            header.backgroundConfiguration.rects[0] = new ColorRect();
            header.backgroundConfiguration.rects[0].rectType = RectType.Border;
            header.backgroundConfiguration.rects[0].borderWidth = new Vector4(0, 0, 0, 1);
            header.backgroundConfiguration.rects[0].color = new Color(0.22f, 0.22f, 0.22f, 1f);
            header.contentPaddingLeft = 6;
            header.contentPaddingRight = 6;
            header.contentPaddingTop = 2;
            header.contentPaddingBottom = 2;
            enableHeader = false;

            list = new ElementList();
            list.contentPaddingLeft = 6;
            list.contentPaddingRight = 6;
            list.contentPaddingTop = 2;//1
            list.contentPaddingBottom = 2;//1
            list.backgroundConfiguration = new BackgroundConfiguration();

            element = new Element();
            element.elementHeaderBackgroundConfiguration = new ElementBackgroundConfiguration();
            element.elementHeaderBackgroundConfiguration.rects = new ElementColorRect[1];
            element.elementHeaderBackgroundConfiguration.rects[0] = new ElementColorRect();
            element.elementHeaderBackgroundConfiguration.rects[0].rectType = RectType.ColorRect;
            element.elementHeaderBackgroundConfiguration.rects[0].drawType = DrawType.DrawWhenSelected;
            element.elementHeaderBackgroundConfiguration.rects[0].color = new Color(0.172549f, 0.3647059f, 0.5294118f, 1f);

            element.elementBodyBackgroundConfiguration = new BackgroundConfiguration();
            element.elementBodyBackgroundConfiguration.rects = new ColorRect[0];


            element.collapsedElementHeight = 20;
            element.headerPaddingRight = 6;
            element.headerPaddingLeft = 0;
            element.headerPaddingTop = 0;
            element.headerPaddingBottom = 0;

            dragHandle = new DragHandle();
            dragHandle.paddingLeft = 5;
            dragHandle.paddingBottom = 6;
            dragHandle.width = 10;
            dragHandle.height = 6;
            dragHandle.allocatedHorizontalSpace = 20;
            dragHandle.guiStyle = new GUIStyle(DRAGGING_HANDLE_STYLE_NAME);

            foldout = new Foldout();
            foldout.width = 13;
            foldout.height = 13;
            foldout.paddingBottom = 3;
            foldout.paddingLeft = 3;
            foldout.allocatedHorizontalSpace = 20;
            foldout.guiStyle = new GUIStyle(FOLDOUT);

            removeElementButton = new RemoveElementButton();
            removeElementButton.height = 20;
            removeElementButton.width = 20;
            removeElementButton.paddingRight = 0;
            removeElementButton.paddingLeft = 6;
            removeElementButton.allocatedHorizontalSpace = 26;
            removeElementButton.guiStyle = new GUIStyle(GUI.skin.label);
            removeElementButton.guiStyle.alignment = TextAnchor.MiddleCenter;
            removeElementButton.guiStyle.fontSize = 16;
            removeElementButton.guiStyle.fontStyle = FontStyle.Bold;
            removeElementButton.content = new GUIContent("X");

            pagination = new Pagination();
            pagination.contentPaddingLeft = 6;
            pagination.contentPaddingRight = 6;
            pagination.contentPaddingTop = 0;
            pagination.contentPaddingBottom = 4;
            pagination.backgroundConfiguration = new BackgroundConfiguration();
            pagination.backgroundConfiguration.rects = new ColorRect[1];
            pagination.backgroundConfiguration.rects[0] = new ColorRect();
            pagination.backgroundConfiguration.rects[0].rectType = RectType.Border;
            pagination.backgroundConfiguration.rects[0].borderWidth = new Vector4(0, 1, 0, 0);
            pagination.backgroundConfiguration.rects[0].color = new Color(0.22f, 0.22f, 0.22f, 1f);
            pagination.labelStyle = new GUIStyle(GUI.skin.label);
            pagination.labelStyle.alignment = TextAnchor.MiddleCenter;
            pagination.buttonStyle = new GUIStyle(FOOTER_BUTTON_STYLE_NAME);
            pagination.firstPageContent = new GUIContent("<<");
            pagination.previousPageContent = new GUIContent("<");
            pagination.nextPageContent = new GUIContent(">");
            pagination.lastPageContent = new GUIContent(">>");
            pagination.buttonsWidth = 25;
            pagination.buttonsHeight = 16;
            pagination.labelHeight = 20;
            pagination.height = 20;

            enableFooterAddButton = true;
            enableFooterRemoveButton = true;

            footerButtons = new FooterButtons();
            footerButtons.marginRight = 10;
            footerButtons.spaceBetweenButtons = 0;
            footerButtons.paddingTop = 4;
            footerButtons.paddingLeft = 4;
            footerButtons.paddingRight = 4;
            footerButtons.buttonsWidth = 25;
            footerButtons.buttonsHeight = 16;
            footerButtons.buttonStyle = new GUIStyle(FOOTER_BUTTON_STYLE_NAME);
            footerButtons.addButton = EditorGUIUtility.TrIconContent("Toolbar Plus");
            footerButtons.removeButton = EditorGUIUtility.TrIconContent("Toolbar Minus");
            footerButtons.height = 20;
            footerButtons.backgroundConfiguration = new BackgroundConfiguration();
            footerButtons.backgroundConfiguration.rects = new ColorRect[2];
            footerButtons.backgroundConfiguration.rects[0] = new ColorRect();
            footerButtons.backgroundConfiguration.rects[1] = new ColorRect();
            footerButtons.backgroundConfiguration.rects[0].paddingTop = -1;
            footerButtons.backgroundConfiguration.rects[0].paddingRight = 0;
            footerButtons.backgroundConfiguration.rects[0].paddingLeft = 0;
            footerButtons.backgroundConfiguration.rects[0].paddingBottom = 1;
            footerButtons.backgroundConfiguration.rects[0].borderWidth = new Vector4(1, 0 ,1 ,1);
            footerButtons.backgroundConfiguration.rects[0].rectType = RectType.ColorRect;
            footerButtons.backgroundConfiguration.rects[0].color = new Color(0.302f, 0.302f, 0.302f, 1f);
            footerButtons.backgroundConfiguration.rects[1].paddingTop = -1;
            footerButtons.backgroundConfiguration.rects[1].paddingRight = 0;
            footerButtons.backgroundConfiguration.rects[1].paddingLeft = 0;
            footerButtons.backgroundConfiguration.rects[1].paddingBottom = 0;
            footerButtons.backgroundConfiguration.rects[1].borderWidth = new Vector4(1, 0, 1, 1);
            footerButtons.backgroundConfiguration.rects[1].borderRadius = new Vector4(0,0,4,4);
            footerButtons.backgroundConfiguration.rects[1].rectType = RectType.Border;
            footerButtons.backgroundConfiguration.rects[1].color = new Color(0.141f, 0.141f, 0.141f, 1f);



            globalBackground = new BackgroundConfiguration();
            globalBackground.rects = new ColorRect[2];
            globalBackground.rects[0] = new ColorRect();
            globalBackground.rects[1] = new ColorRect();
            globalBackground.rects[0].paddingTop = 1;
            globalBackground.rects[0].paddingRight = 1;
            globalBackground.rects[0].paddingLeft = 1;
            globalBackground.rects[0].paddingBottom = 21;
            globalBackground.rects[0].borderWidth = Vector4.one;
            globalBackground.rects[0].borderRadius = Vector4.one * 4;
            globalBackground.rects[0].rectType = RectType.ColorRect;
            globalBackground.rects[0].color = new Color(0.302f, 0.302f, 0.302f, 1f);
            globalBackground.rects[1].paddingTop = 0;
            globalBackground.rects[1].paddingRight = 0;
            globalBackground.rects[1].paddingLeft = 0;
            globalBackground.rects[1].paddingBottom = 20;
            globalBackground.rects[1].borderWidth = Vector4.one;
            globalBackground.rects[1].borderRadius = Vector4.one * 4;
            globalBackground.rects[1].rectType = RectType.Border;
            globalBackground.rects[1].color = new Color(0.141f, 0.141f, 0.141f, 1);
        }

        [Serializable]
        public class Header: ElementList
        {
            public float height;
            public GUIStyle labelStyle;
        }

        [Serializable]
        public class ElementList
        {
            public float contentPaddingLeft;
            public float contentPaddingRight;
            public float contentPaddingTop;
            public float contentPaddingBottom;
            public BackgroundConfiguration backgroundConfiguration;
        }

        [Serializable]
        public class Element
        {
            public float collapsedElementHeight;
            public float headerPaddingLeft; //0 because we draw DragHandle with its own paddingLeft
            public float headerPaddingRight;
            public float headerPaddingTop;
            public float headerPaddingBottom;
            public float bodyPaddingLeft; //0 because we draw DragHandle with its own paddingLeft
            public float bodyPaddingRight;
            public float bodyPaddingTop;
            public float bodyPaddingBottom;

            public ElementBackgroundConfiguration elementHeaderBackgroundConfiguration;
            public BackgroundConfiguration elementBodyBackgroundConfiguration;
        }

        [Serializable]
        public class DragHandle
        {
            public float paddingLeft;
            public float paddingBottom;
            public float width;
            public float height;
            public float allocatedHorizontalSpace; 
            public GUIStyle guiStyle;
        }

        [Serializable]
        public class Foldout
        {
            public GUIStyle guiStyle;
            public float width;
            public float height;
            public float paddingBottom;
            public float paddingLeft;
            public float allocatedHorizontalSpace;
        }

        [Serializable]
        public class RemoveElementButton
        {
            public GUIStyle guiStyle;
            public float width;
            public float height;
            public float paddingRight; 
            public float paddingLeft;
            public float allocatedHorizontalSpace;
            public GUIContent content;
        }

        [Serializable]
        public class Pagination: ElementList
        {
            public GUIStyle labelStyle;
            public GUIStyle buttonStyle;
            public GUIContent firstPageContent;
            public GUIContent previousPageContent;
            public GUIContent nextPageContent;
            public GUIContent lastPageContent;
            public float buttonsWidth;
            public float buttonsHeight;
            public float labelHeight;
            public float height;
        }

        [Serializable]
        public class FooterButtons
        {
            public float marginRight;
            public float spaceBetweenButtons;
            public float paddingTop;
            public float paddingLeft;
            public float paddingRight;
            public GUIStyle buttonStyle;
            public float buttonsWidth;
            public float buttonsHeight;
            public GUIContent addButton;
            public GUIContent removeButton;
            public float height;
            public BackgroundConfiguration backgroundConfiguration;

        }

        [Serializable]
        public class ElementBackgroundConfiguration
        {
            public ElementColorRect[] rects;
        }


        [Serializable]
        public class ElementColorRect: ColorRect
        {
            public DrawType drawType;
        }

        [Serializable]
        public class BackgroundConfiguration
        {
            public ColorRect[] rects;
        }

        [Serializable]
        public class ColorRect
        {
            public float paddingLeft;
            public float paddingRight;
            public float paddingTop;
            public float paddingBottom;
            public Color color;
            public Vector4 borderWidth;
            public Vector4 borderRadius;
            public RectType rectType;
        }

        [Serializable]
        public enum DrawType
        {
            DrawAlways,
            DrawWhenSelected,
            DrawWhenUnselected
        }

        [Serializable]
        public enum RectType
        {
            ColorRect,
            Border,
            RoundRect,
        }

        [Serializable]
        public enum ConstrainsType
        {
            PaddingLeft,
            PaddingRight,
            PaddingTop,
            PaddingBottom,
        }

        [Serializable]
        public class PropertyFieldStyle
        {
            public float paddingLeft;
            public float paddingRight;
            public float paddingTop;
            public float paddingBottom;
            public float fieldHeight;
        }
    }
}
