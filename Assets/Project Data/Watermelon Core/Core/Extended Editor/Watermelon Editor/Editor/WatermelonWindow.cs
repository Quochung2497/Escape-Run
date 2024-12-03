using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Watermelon
{
    public abstract class WatermelonWindow : EditorWindow
    {
        private static WatermelonWindow watermelonEditor;

        private bool isStyleInited;
        protected bool IsStyleInited
        {
            get { return isStyleInited; }
        }

        protected virtual void OnEnable()
        {
            watermelonEditor = this;

            EditorApplication.playModeStateChanged += LogPlayModeState;
            EditorSceneManager.activeSceneChangedInEditMode += ActiveSceneChanged;
        }

        protected void InitStyles()
        {
            if (isStyleInited)
                return;

            Styles();

            isStyleInited = true;
        }

        protected void ForceInitStyles()
        {
            isStyleInited = false;

            InitStyles();
        }

        protected virtual void Styles()
        {

        }

        private void ActiveSceneChanged(Scene scene1, Scene scene2)
        {
            //ForceInitStyles();
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                if (watermelonEditor != null)
                {
                    //watermelonEditor.ForceInitStyles();
                }
            }
        }
    }
}
