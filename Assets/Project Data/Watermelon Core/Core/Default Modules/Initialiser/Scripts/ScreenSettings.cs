#pragma warning disable 0649
#pragma warning disable 0414

using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class ScreenSettings
    {
        [Header("Frame Rate")]
        [SerializeField] bool setFrameRateAutomatically = false;

        [Space]
        [SerializeField] AllowedFrameRates defaultFrameRate = AllowedFrameRates.Rate60;
        [SerializeField] AllowedFrameRates batterySaveFrameRate = AllowedFrameRates.Rate30;

        [Header("Sleep")]
        [SerializeField] int sleepTimeout = -1;

        public void Initialise()
        {
            Screen.sleepTimeout = sleepTimeout;

            if (setFrameRateAutomatically)
            {
                uint numerator = Screen.currentResolution.refreshRateRatio.numerator;
                uint denominator = Screen.currentResolution.refreshRateRatio.denominator;

                if(numerator != 0 && denominator != 0)
                {
                    Application.targetFrameRate = Mathf.RoundToInt(numerator / denominator);
                }
                else
                {
                    Application.targetFrameRate = (int)defaultFrameRate;
                }
            }
            else
            {
#if UNITY_IOS
                if(UnityEngine.iOS.Device.lowPowerModeEnabled)
                {
                    Application.targetFrameRate = (int)batterySaveFrameRate;
                }
                else
                {
                    Application.targetFrameRate = (int)defaultFrameRate;
                }    
#else
                Application.targetFrameRate = (int)defaultFrameRate;
#endif
            }
        }

        private enum AllowedFrameRates
        {
            Rate30 = 30,
            Rate60 = 60,
            Rate90 = 90,
            Rate120 = 120,
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
