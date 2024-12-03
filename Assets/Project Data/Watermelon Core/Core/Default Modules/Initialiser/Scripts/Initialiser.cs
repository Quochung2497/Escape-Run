#pragma warning disable 0649

using UnityEngine;

namespace Watermelon
{
    [DefaultExecutionOrder(-999)]
    [HelpURL("https://docs.google.com/document/d/1ORNWkFMZ5_Cc-BUgu9Ds1DjMjR4ozMCyr6p_GGdyCZk")]
    public class Initialiser : MonoBehaviour
    {
        [SerializeField] ProjectInitSettings initSettings;
        [SerializeField] Canvas systemCanvas;

        [Space]
        [SerializeField] ScreenSettings screenSettings;

        public static Canvas SystemCanvas;
        public static GameObject InitialiserGameObject;

        public static bool IsInititalized { get; private set; }
        public static bool IsStartInitialized { get; private set; }

        public void Awake()
        {
            screenSettings.Initialise();

            if (!IsInititalized)
            {
                IsInititalized = true;

                SystemCanvas = systemCanvas;
                InitialiserGameObject = gameObject;

                DontDestroyOnLoad(gameObject);

                initSettings.Init(this);
            }
        }

        public void Start()
        {
            Initialise(true);
        }

        public void Initialise(bool loadingScene)
        {
            if (!IsStartInitialized)
            {
                // Initialise components
                initSettings.StartInit(this);

                // Create audio listener
                AudioController.CreateAudioListener();

                IsStartInitialized = true;

                if (loadingScene)
                {
                    GameLoading.LoadGameScene();
                }
                else
                {
                    GameLoading.SimpleLoad();
                }
            }
        }

        private void OnDestroy()
        {
            IsInititalized = false;

#if UNITY_EDITOR
            SaveController.ForceSave();
#endif
        }

        private void OnApplicationFocus(bool focus)
        {
#if !UNITY_EDITOR
            if(!focus) SaveController.Save();
#endif
        }
    }
}

// -----------------
// Initialiser v 0.4.2
// -----------------

// Changelog
// v 0.4.2
// • Added loading scene logic
// v 0.4.1
// • Fixed error on module remove
// v 0.3.1
// • Added link to the documentation
// • Initializer renamed to Initialiser
// • Fixed problem with recompilation
// v 0.2
// • Added sorting feature
// • Initialiser MonoBehaviour will destroy after initialization
// v 0.1
// • Added basic version
